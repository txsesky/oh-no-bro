using Game.Components;
using Game.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class TransformUpdateSystem : IEcsRunSystem
    {
        [EcsWorld] 
        private readonly EcsWorld _world = default;
        
        [EcsShared] 
        private readonly SharedData _sharedData = default;
        
        [EcsFilter(typeof(HierarchyData),typeof(LocalToWorldData))]
        private readonly EcsFilter _hierarchyGroup = default;
        
        [EcsFilter(typeof(TransformRef),typeof(LocalToWorldData))]
        private readonly EcsFilter _transformGroup = default;
        
        [EcsPool] 
        private readonly EcsPool<TransformRef> _transformPool = default;

        [EcsPool] 
        private readonly EcsPool<LocalToParentData> _localToParentPool = default;
        
        [EcsPool] 
        private readonly EcsPool<LocalToWorldData> _localToWorldPool = default;
        
        [EcsPool] 
        private readonly EcsPool<HierarchyData> _hierarchyPool = default;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _hierarchyGroup)
            {
                ref var hierarchyData = ref _hierarchyPool.Get(entity);
                
                if (!hierarchyData.Parent.IsNull()) //TODO: updating transform locally and globally
                    continue;
                
                var parentTM = hierarchyData.Parent.IsNull() ? _localToWorldPool.Get(hierarchyData.Parent).Value : Matrix4x4.identity;

                Transform(parentTM, entity);
            }
            
            foreach (var entity in _transformGroup)
            {
                ref var transform = ref _transformPool.Get(entity);
                ref var localToWorld = ref _localToWorldPool.Get(entity);

                transform.Value.position = localToWorld.Position;
            }
        }

        private void Transform(Matrix4x4 parent,int entity)
        {
            _localToWorldPool.Get(entity).Value = _localToParentPool.Get(entity).Value * parent;
            
            var child = _hierarchyPool.Get(entity).FirstChild;

            while (child != -1)
            {
                Transform(_localToWorldPool.Get(entity).Value, child);
                child = _hierarchyPool.Get(child).NextSibling;
            }
        }
    }
}