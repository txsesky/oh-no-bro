using Game.Components;
using Game.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class DisposeSystem : IEcsRunSystem
    {
        [EcsWorld] 
        private readonly EcsWorld _world = default;

        [EcsFilter(typeof(DisposeData))]
        [EcsFilterExclude(typeof(HierarchyData), typeof(TransformRef))]
        private readonly EcsFilter _disposedGroup = default;
        
        [EcsFilter(typeof(DisposeData), typeof(TransformRef))]
        [EcsFilterExclude(typeof(HierarchyData))]
        private readonly EcsFilter _disposedWithTransformGroup = default;
        
        [EcsFilter(typeof(DisposeData), typeof(TransformRef), typeof(HierarchyData))]
        private readonly EcsFilter _disposedWithTransformHierarchyGroup = default;

        [EcsPool] 
        private readonly EcsPool<TransformRef> _transformPool = default;

        [EcsPool] 
        private readonly EcsPool<HierarchyData> _hierarchyPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var disposedEntity in _disposedGroup)
            {
                _world.DelEntity(disposedEntity);
            }
            
            foreach (var disposedEntity in _disposedWithTransformGroup)
            {
                Object.Destroy(_transformPool.Get(disposedEntity).Value);
                _world.DelEntity(disposedEntity);
            }

            foreach (var disposedEntity in _disposedWithTransformHierarchyGroup)
            {
                ref var hierarchyData = ref _hierarchyPool.Get(disposedEntity);

                var firstChild = -1;
                
                if (_hierarchyPool.Has(hierarchyData.Parent))
                {
                    ref var parentHierarchyData = ref _hierarchyPool.Get(hierarchyData.Parent);
                    firstChild = parentHierarchyData.FirstChild;
                }

                if (!hierarchyData.NextSibling.IsNull())
                {
                    ref var nextHierarchyData = ref _hierarchyPool.Get(hierarchyData.NextSibling);
                    
                    if (!hierarchyData.PrevSibling.IsNull())
                    {
                        ref var prevHierarchyData = ref _hierarchyPool.Get(hierarchyData.PrevSibling);
                        prevHierarchyData.NextSibling = hierarchyData.NextSibling;
                        nextHierarchyData.PrevSibling = hierarchyData.PrevSibling;
                    }
                    else
                    {
                        nextHierarchyData.PrevSibling = -1;
                        
                        firstChild = nextHierarchyData.NextSibling;
                    }
                }
                else
                {
                    if (!hierarchyData.PrevSibling.IsNull())
                    {
                        ref var prevHierarchyData = ref _hierarchyPool.Get(hierarchyData.PrevSibling);
                        prevHierarchyData.NextSibling = -1;
                    }
                    else
                    {
                        if (_hierarchyPool.Has(hierarchyData.Parent))
                        {
                            firstChild = -1;
                        }
                    }
                }
                
                if (_hierarchyPool.Has(hierarchyData.Parent))
                {
                    ref var parentHierarchyData = ref _hierarchyPool.Get(hierarchyData.Parent);
                    parentHierarchyData.FirstChild = firstChild;
                    parentHierarchyData.ChildrenCount -= 1;
                }

                var next = hierarchyData.FirstChild;
                
                while (next != -1)
                {
                    var curr = next;
                    next = _hierarchyPool.Get(next).NextSibling;
                    _world.DelEntity(curr);
                }
                
                Object.Destroy(_transformPool.Get(disposedEntity).Value);
                _world.DelEntity(disposedEntity);
            }
        }
    }
}