using Game.Components;
using Game.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class ParentingSystem : IEcsRunSystem
    {
        [EcsWorld] 
        private readonly EcsWorld _world = default;
        
        [EcsFilter(typeof(SetParentData))] 
        private readonly EcsFilter _pendingGroup;
        
        [EcsPool]
        private readonly EcsPool<SetParentData> _setParentPool = default;
        
        [EcsPool]
        private readonly EcsPool<TransformRef> _transformPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _pendingGroup)
            {
                ref var setParentData = ref _setParentPool.Get(entity);

                entity.SetParent(setParentData.Entity, _world, setParentData.LocalTranslation);

                _setParentPool.Del(entity);
            }
        }
    }
}