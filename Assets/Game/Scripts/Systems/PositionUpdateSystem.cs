using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class PositionUpdateSystem : IEcsRunSystem
    {
        [EcsFilter(typeof(TransformData),typeof(PositionData))]
        private readonly EcsFilter _entities = default;
        
        [EcsPool] 
        private readonly EcsPool<TransformData> _transformPool = default;
        
        [EcsPool]
        private readonly EcsPool<PositionData> _positionPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _entities)
            {
                ref var transform = ref _transformPool.Get(entity);
                ref var position = ref _positionPool.Get(entity);

                transform.TransformRef.position = position.Value;
            }
        }
    }
}