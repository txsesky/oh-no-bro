using System.Collections.Generic;
using Game.Components;
using Game.Factory;
using Game.Networking;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Photon.Pun;
using UnityEngine;

namespace Game.Systems
{
    public class SpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private string _photonViewPrefab = "Prefabs/Network/PhotonView";
        private string _shopPrefab = "Prefabs/Buildings/Shop";
        private float _spawnPointsDistance = 15f;
        private float _spawnPointsHeight = 0f;
        private float _shopColliderRadius = 3f;
        private float _shopTriggerRadius = 4f;

        private readonly List<Vector3> _spawnPointsList = new List<Vector3>();

        [EcsWorld] private readonly EcsWorld _world = default;

        [EcsShared] private readonly SharedData _sharedData = default;

        [EcsPool] private readonly EcsPool<TransformRef> _transformPool = default;

        [EcsPool] private readonly EcsPool<InputData> _inputPool = default;

        [EcsPool] private readonly EcsPool<PhotonViewRef> _photonViewPool = default;

        [EcsPool] private readonly EcsPool<NetSyncPositionData> _networkSyncPositionPool = default;

        [EcsPool] private readonly EcsPool<SphereColliderRef> _colliderPool = default;

        [EcsPool] private readonly EcsPool<RigidbodyRef> _rigidbodyPool = default;

        [EcsPool] private readonly EcsPool<PlayerData> _playerPool = default;

        [EcsPool] private readonly EcsPool<ShopData> _shopPool = default;

        [EcsPool] private readonly EcsPool<SetParentData> _setParentPool = default;

        private bool _arePlayersSpawned;

        public void Init(EcsSystems systems)
        {
            _arePlayersSpawned = false;
            
            if (_sharedData.GameState.GameMode == GameMode.Client)
            {
                GenerateSpawnPoints();
                PhotonNetwork.Instantiate(_photonViewPrefab, Vector3.zero, Quaternion.identity);
            }
            
            SpawnEnvironment();
        }

        public void Run(EcsSystems systems)
        {
            if (_arePlayersSpawned)
                return;

            if (_sharedData.GameState.GameMode == GameMode.SinglePlayer)
            {
                _arePlayersSpawned = true;

                var entity = _world.NewEntity();
                
                var character = new Character(entity, _world, new Vector3(_spawnPointsDistance, 0,0), Quaternion.identity);
               
                _inputPool.Add(entity);
                _playerPool.Add(entity);

                return;
            }

            var photonViews = Object.FindObjectsOfType<PhotonView>();

            if (photonViews.Length != PhotonNetwork.PlayerList.Length)
                return;

            _arePlayersSpawned = true;

            for (int i = 0; i < photonViews.Length; i++)
            {
                var player = PhotonNetwork.PlayerList[i];
                var photonView = photonViews[0];

                foreach (var pV in photonViews)
                {
                    if (Equals(pV.Owner, player))
                    {
                        photonView = pV;
                    }
                }

                var entity = _world.NewEntity();
                
                var character = new Character(entity, _world, _spawnPointsList[i], Quaternion.identity);//TODO createmethod

                ref var networkSyncPositionData = ref _networkSyncPositionPool.Add(entity);
                networkSyncPositionData.CurrentValue = _spawnPointsList[i];

                ref var photonViewData = ref _photonViewPool.Add(entity);
                photonViewData.Value = photonView;

                if (photonView.IsMine)
                {
                    _inputPool.Add(entity);
                    _playerPool.Add(entity);
                }

                photonView.GetComponent<SyncTransform>().SetCharacterEntity(entity, _world);
            }
        }

        private void GenerateSpawnPoints()
        {
            var playersCount = PhotonNetwork.PlayerList.Length;
            var baseAngle = 360f / playersCount;

            _spawnPointsList.Clear();

            for (int i = 0; i < playersCount; i++)
            {
                var angle = i * baseAngle;
                var spawnPoint = new Vector3(_spawnPointsDistance * Mathf.Cos(Mathf.Deg2Rad * angle),
                    _spawnPointsHeight,
                    _spawnPointsDistance * Mathf.Sin(Mathf.Deg2Rad * angle));
                _spawnPointsList.Add(spawnPoint);
            }
        }

        private void SpawnEnvironment()
        {
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
        }
    }
}