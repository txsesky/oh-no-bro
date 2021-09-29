using System.Collections.Generic;
using Game.Components;
using Game.Factory;
using Game.Networking;
using Game.UI.HUD;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using Photon.Pun;
using UnityEngine;

namespace Game.Systems {
	sealed class SpawnSystem : IEcsInitSystem, IEcsRunSystem {
		string _photonViewPrefab = "Prefabs/Network/PhotonView";
		string _shopPrefab = "Prefabs/Buildings/Shop";
		string _shopPanelPrefab = "Prefabs/UI/Shop_Panel";
		string _shopItemElmntPrefab = "Prefabs/UI/ShopItemElmnt";
		float _spawnPointsDistance = 15f;
		float _spawnPointsHeight = 0f;
		float _shopColliderRadius = 3f;
		float _shopTriggerRadius = 4f;

		readonly List<Vector3> _spawnPointsList = new List<Vector3>();

		[EcsShared]
		readonly SharedData _sharedData = default;
		
		readonly EcsWorld _world = default;
		readonly EcsPool<TransformRefData> _transformPool = default;
		readonly EcsPool<InputData> _inputPool = default;
		readonly EcsPool<PhotonViewRef> _photonViewPool = default;
		readonly EcsPool<NetSyncPositionData> _networkSyncPositionPool = default;
		readonly EcsPool<SphereColliderRefData> _colliderPool = default;
		readonly EcsPool<RigidbodyRefData> _rigidbodyPool = default;
		readonly EcsPool<PlayerData> _playerPool = default;
		readonly EcsPool<ShopData> _shopPool = default;
		readonly EcsPool<SetParentData> _setParentPool = default;
		readonly EcsPool<UIShopPanelRef> _uiShopPanelPool = default;
		readonly EcsPool<UIShopItemElmntRef> _uiShopItemElmntPool = default;

		bool
			_arePlayersSpawned; //TODO split to PlayerSpawnSystem & PlayersSpawnSystem / EnvironmentLoadSystem / UILoadSystem

		public void Init(EcsSystems systems) {
			_arePlayersSpawned = false;

			if (_sharedData.GameState.GameMode == Idents.GameModes.Client) {
				GenerateSpawnPoints();
				PhotonNetwork.Instantiate(_photonViewPrefab, Vector3.zero, Quaternion.identity);
			}

			SpawnEnvironment();
			SpawnUIShopPanel();
		}

		public void Run(EcsSystems systems) {
			//respawn logic here

			if (_arePlayersSpawned)
				return;

			if (_sharedData.GameState.GameMode == Idents.GameModes.SinglePlayer) {
				_arePlayersSpawned = true;

				var entity = _world.NewEntity();

				var character = new Character(entity, _world, new Vector3(_spawnPointsDistance, 0, 0),
					Quaternion.identity);

				_inputPool.Add(entity);
				_playerPool.Add(entity);

				return;
			}

			var photonViews = Object.FindObjectsOfType<PhotonView>();

			if (photonViews.Length != PhotonNetwork.PlayerList.Length)
				return;

			_arePlayersSpawned = true;

			for (int i = 0; i < photonViews.Length; i++) {
				var player = PhotonNetwork.PlayerList[i];
				var photonView = photonViews[0];

				foreach (var pV in photonViews) {
					if (Equals(pV.Owner, player)) {
						photonView = pV;
					}
				}

				var entity = _world.NewEntity();

				var character =
					new Character(entity, _world, _spawnPointsList[i], Quaternion.identity); //TODO createmethod

				ref var networkSyncPositionData = ref _networkSyncPositionPool.Add(entity);
				networkSyncPositionData.CurrentValue = _spawnPointsList[i];

				ref var photonViewData = ref _photonViewPool.Add(entity);
				photonViewData.Value = photonView;

				if (photonView.IsMine) {
					_inputPool.Add(entity);
					_playerPool.Add(entity);
				}

				photonView.GetComponent<SyncTransform>().SetCharacterEntity(entity, _world);
			}
		}

		void GenerateSpawnPoints() {
			var playersCount = PhotonNetwork.PlayerList.Length;
			var baseAngle = 360f / playersCount;

			_spawnPointsList.Clear();

			for (int i = 0; i < playersCount; i++) {
				var angle = i * baseAngle;
				var spawnPoint = new Vector3(_spawnPointsDistance * Mathf.Cos(Mathf.Deg2Rad * angle),
					_spawnPointsHeight,
					_spawnPointsDistance * Mathf.Sin(Mathf.Deg2Rad * angle));
				_spawnPointsList.Add(spawnPoint);
			}
		}

		void SpawnEnvironment() {
			//Shop
			var entity = _world.NewEntity();

			var go = Object.Instantiate(Resources.Load(_shopPrefab)) as GameObject;

			if (go == null)
				return;

			ref var transformData = ref _transformPool.Add(entity);
			transformData.Value = go.transform;

			//TODO move to system
			ref var colliderData = ref _colliderPool.Add(entity);
			colliderData.Value = go.AddComponent<SphereCollider>();
			colliderData.Value.radius = _shopTriggerRadius;
			colliderData.Value.isTrigger = true;

			ref var rbData = ref _rigidbodyPool.Add(entity);
			rbData.Value = go.AddComponent<Rigidbody>();
			rbData.Value.isKinematic = true;

			go.AddComponent<OnTriggerEnterChecker>();
			go.AddComponent<OnTriggerExitChecker>();

			_shopPool.Add(entity);

			var child = _world.NewEntity();

			var childGO = new GameObject("Collider");

			if (childGO == null)
				return;

			ref var tData = ref _transformPool.Add(child);
			tData.Value = childGO.transform;

			ref var cData = ref _colliderPool.Add(child);
			cData.Value = childGO.AddComponent<SphereCollider>();
			cData.Value.radius = _shopColliderRadius;

			ref var setParentData = ref _setParentPool.Add(child);
			setParentData.Entity = entity;
			setParentData.LocalTranslation = Vector3.zero;
		}

		void SpawnUIShopPanel() {
			var entity = _world.NewEntity();

			var root = GameObject.Find("UI");
			var go = Object.Instantiate(Resources.Load(_shopPanelPrefab), root.transform) as GameObject;

			if (go == null)
				return;

			ref var transformData = ref _transformPool.Add(entity);
			transformData.Value = go.transform;

			ref var shopPanelData = ref _uiShopPanelPool.Add(entity);
			shopPanelData.Value = go.GetComponent<UIShopPanel>();
			shopPanelData.Value.Hide();

			SpawnUIShopItem(shopPanelData.Value.GetItemsContainer(), new ShopItemData {
				Name = "Movement Speed",
				Level = 1,
				Cost = 10,
				Stats = new[] {Stat.MS},
				StatsModifiers = new[] {
					new StatModifier() {
						AddVal = 1
					}
				}
			});
		}

		void SpawnUIShopItem(Transform root, ShopItemData shopItemData) {
			var entity = _world.NewEntity();

			var go = Object.Instantiate(Resources.Load(_shopItemElmntPrefab), root) as GameObject;

			if (go == null)
				return;

			ref var transformData = ref _transformPool.Add(entity);
			transformData.Value = go.transform;

			ref var shopItemElmntData = ref _uiShopItemElmntPool.Add(entity);
			shopItemElmntData.Value = go.GetComponent<UIShopItemElmnt>();
			shopItemElmntData.Value.Setup(shopItemData);

			shopItemElmntData.Value.GetSelectable().gameObject.AddComponent<EcsUguiClickAction>();
		}
	}
}