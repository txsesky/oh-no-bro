using System;
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

        [EcsFilter(typeof(PositionData), typeof(MovementData), typeof(ColliderData), typeof(PlayerData))]
        private readonly EcsFilter _movingEntities = default;
        
        [EcsFilter(typeof(PositionData),typeof(ColliderData))]
        private readonly EcsFilter _colliderEntities = default;
        
        [EcsPool] 
        private readonly EcsPool<PositionData> _positionPool = default;

        [EcsPool] 
        private readonly EcsPool<MovementData> _movementPool = default;
        
        [EcsPool] 
        private readonly EcsPool<ColliderData> _colliderPool = default;
        
        [EcsPool] 
        private readonly EcsPool<PlayerData> _playerPool = default;

        public void Run(EcsSystems systems)
        {
            foreach (var movingEntity in _movingEntities)
            {
                ref var player = ref _playerPool.Get(movingEntity);
                if(!player.PhotonView.IsMine)
                    continue;
                
                ref var positionA = ref _positionPool.Get(movingEntity);
                ref var movement = ref _movementPool.Get(movingEntity);
                ref var colliderA = ref _colliderPool.Get(movingEntity);

                if (movement.Direction.x == 0f && movement.Direction.y == 0f) 
                    continue;

                var posAVec3 = positionA.Value + new Vector3(movement.Direction.x, 0, movement.Direction.y) * movement.Speed * _sharedData.DeltaTime;
                var posAVec2 = new Vector2(posAVec3.x, posAVec3.z);

                var collided = false;
                
                foreach (var colliderEntity in _colliderEntities)
                {
                    if(movingEntity == colliderEntity)  
                        continue;
                        
                    ref var colliderB = ref _colliderPool.Get(colliderEntity);
                    ref var positionB = ref _positionPool.Get(colliderEntity);
                    
                    var posBVec3 = positionB.Value;
                    var posBVec2 = new Vector2(posBVec3.x, posBVec3.z);

                    collided = AreOverlapping(colliderA.Radius, colliderB.Radius, posAVec2, posBVec2);
                        
                    if (collided)
                        break;
                }

                if (collided)
                {
                    movement.Direction = Vector2.zero;
                }
                else
                {
                    positionA.Value = posAVec3;
                }
            }
        }

        private bool AreOverlapping(float radiusA, float radiusB, Vector2 posA, Vector2 posB)
        {
            var dx = posA.x - posB.x;
            var dy = posA.y - posB.y;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            return distance < radiusA + radiusB;
        }
    }
}