using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        [EcsShared] 
        private readonly SharedData _sharedData = default;

        [EcsFilter(typeof(LocalToWorldData), typeof(MovementData), typeof(PlayerData))]
        private readonly EcsFilter _movingEntityGroup = default;

        [EcsPool] 
        private readonly EcsPool<LocalToWorldData> _positionPool = default;

        [EcsPool] 
        private readonly EcsPool<MovementData> _movementPool = default;

        [EcsPool] 
        private readonly EcsPool<PlayerData> _playerPool = default;

        public void Run(EcsSystems systems)
        {
            foreach (var movingEntity in _movingEntityGroup)
            {
                ref var player = ref _playerPool.Get(movingEntity);
                if(!player.PhotonView.IsMine)
                    continue;
                
                ref var localToWorld = ref _positionPool.Get(movingEntity);
                ref var movement = ref _movementPool.Get(movingEntity);

                if (movement.Direction.x == 0f && movement.Direction.y == 0f) 
                    continue;
                
                localToWorld.Position += new Vector3(movement.Direction.x, 0, movement.Direction.y) * movement.Speed * _sharedData.DeltaTime;
            }
        }
    }
}