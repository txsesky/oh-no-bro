using System.Collections.Generic;
using Game.Components;
using Game.Factory;
using Game.Networking;
using Leopotam.EcsLite;
using Photon.Pun;
using UnityEngine;

namespace Game.Systems {
	sealed class PlayersSpawnSystem : IEcsInitSystem, IEcsRunSystem {
		string _photonViewPrefab = "Prefabs/Network/PhotonView";
		float _spawnPointsDistance = 15f;
		float _spawnPointsHeight = 0f;

		readonly List<Vector3> _spawnPointsList = new List<Vector3>();

		readonly EcsWorld _world = default;
		readonly EcsPool<InputData> _inputPool = default;
		readonly EcsPool<PhotonViewRef> _photonViewPool = default;
		readonly EcsPool<NetSyncPositionData> _networkSyncPositionPool = default;
		readonly EcsPool<PlayerData> _playerPool = default;

		bool _arePlayersSpawned;

		public void Init(EcsSystems systems) {
			_arePlayersSpawned = false;
			
			GenerateSpawnPoints();
			PhotonNetwork.Instantiate(_photonViewPrefab, Vector3.zero, Quaternion.identity);
		}

		public void Run(EcsSystems systems) {
			if (_arePlayersSpawned)
				return;

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

				CharacterFactory.CreateCharacter(entity, _world, _spawnPointsList[i], Quaternion.identity);

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
	}
}