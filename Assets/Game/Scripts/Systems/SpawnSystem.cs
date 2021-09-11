using System.Collections.Generic;
using ExitGames.Client.Photon;
using Game.Components;
using Game.Networking;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Photon.Pun;
using UnityEngine;

namespace Game.Systems
{
    public class SpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private string _photonViewPrefab = "Prefabs/Network/PhotonView";
        private string _dwarfPrefab = "Prefabs/Characters/Dwarf";
        private string _shopPrefab = "Prefabs/Buildings/Shop";
        private float _spawnPointsDistance = 15f;
        private float _spawnPointsHeight = 0f;
        private float _movementSpeed = 10f;
        private float _charColliderRadius = 0.5f;
        private float _shopColliderRadius = 3f;
        private float _shopTriggerRadius = 4f;

        private readonly List<Vector3> _spawnPointsList = new List<Vector3>();
        
        [EcsWorld] 
        private readonly EcsWorld _world = default;
        
        [EcsPool] 
        private readonly EcsPool<TransformData> _transformPool = default;
        
        [EcsPool] 
        private readonly EcsPool<InputData> _inputPool = default;
        
        [EcsPool] 
        private readonly EcsPool<MovementData> _movementPool = default;
        
        [EcsPool] 
        private readonly EcsPool<PlayerData> _playerPool = default;
        
        [EcsPool] 
        private readonly EcsPool<PositionData> _positionPool = default;
        
        [EcsPool] 
        private readonly EcsPool<NetSyncPositionData> _networkSyncPositionPool = default;
        
        [EcsPool] 
        private readonly EcsPool<ColliderData> _colliderPool = default;
        
        [EcsPool] 
        private readonly EcsPool<TriggerData> _triggerPool = default;
        
        [EcsPool] 
        private readonly EcsPool<ShopData> _shopPool = default;

        private bool _arePhotonViewsInitiated;

        public void Init(EcsSystems systems)
        {
            _arePhotonViewsInitiated = false;
            GenerateSpawnPoints();
            PhotonNetwork.Instantiate(_photonViewPrefab, Vector3.zero, Quaternion.identity);
            SpawnEnvironment();
        }

        public void Run(EcsSystems systems)
        {
            if(_arePhotonViewsInitiated)
                return;
            
            var photonViews = GameObject.FindObjectsOfType<PhotonView>();
            
            if(photonViews.Length != PhotonNetwork.PlayerList.Length)
                return;
            
            _arePhotonViewsInitiated = true;

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
                
                photonView.GetComponent<SyncPosition>().SetCharacterEntity(entity);

                var go = Object.Instantiate(Resources.Load(_dwarfPrefab)) as GameObject;

                ref var transformData = ref _transformPool.Add(entity);
                transformData.TransformRef = go.transform;

                var spawnPoint = _spawnPointsList[i];
                
                ref var positionData = ref _positionPool.Add(entity);
                ref var networkSyncPositionData = ref _networkSyncPositionPool.Add(entity);
                
                positionData.Value = networkSyncPositionData.CurrentValue = spawnPoint;

                if (photonView.IsMine)
                {
                    ref var inputData = ref _inputPool.Add(entity);
                }

                ref var movementData = ref _movementPool.Add(entity);
                movementData.Speed = _movementSpeed;

                ref var playerData = ref _playerPool.Add(entity);
                playerData.PhotonView = photonView;
                
                ref var colliderData = ref _colliderPool.Add(entity);
                colliderData.Radius = _charColliderRadius;
                
                ref var triggerData = ref _triggerPool.Add(entity);
                triggerData.Radius = _charColliderRadius;
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

        private void GetSpawnPoint(out Vector3 spawnPos, out Quaternion spawnRot)
        {
            var id = PhotonNetwork.LocalPlayer.ActorNumber;
            spawnPos = _spawnPointsList[(id == -1) ? 0 : id % _spawnPointsList.Count];
            spawnRot = Quaternion.identity;
        }

        private void SpawnEnvironment()
        {
            //Shop
            var entity = _world.NewEntity();

            Object.Instantiate(Resources.Load(_shopPrefab));

            ref var colliderData = ref _colliderPool.Add(entity);
            colliderData.Radius = _shopColliderRadius;
            
            ref var triggerData = ref _triggerPool.Add(entity);
            triggerData.Radius = _shopTriggerRadius;
            
            _positionPool.Add(entity);
            _shopPool.Add(entity);
        }
    }
}