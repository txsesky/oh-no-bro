using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Game.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Extensions
{
    public static class EntityExtension
    {
        public static bool IsNull(this int entity)
        {
            return entity == -1;
        }
        
        public static void SetParent(this int child, int parent, EcsWorld ecsWorld, Vector3 localTranslation)
        {
            //TODO: move this all to system
            var hierarchyPool = ecsWorld.GetPool<HierarchyData>();
            var localToParentPool = ecsWorld.GetPool<LocalToParentData>();

            if (!hierarchyPool.Has(parent))
            {
                ref var parentHierarchyData = ref hierarchyPool.Add(parent);
                parentHierarchyData.FirstChild = child;
                parentHierarchyData.NextSibling = -1;
                parentHierarchyData.PrevSibling = -1;
                parentHierarchyData.Parent = -1;

                parentHierarchyData.ChildrenCount += 1;

                if (!hierarchyPool.Has(child))
                {
                    ref var childHierarchyData = ref hierarchyPool.Add(child);
                    childHierarchyData.FirstChild = -1;
                    childHierarchyData.NextSibling = -1;
                    childHierarchyData.PrevSibling = -1;
                    childHierarchyData.Parent = parent;
                }
                else
                {
                    ref var childHierarchyData = ref hierarchyPool.Get(child);
                    childHierarchyData.NextSibling = -1;
                    childHierarchyData.PrevSibling = -1;
                    childHierarchyData.Parent = parent;
                }
            }
            else
            {
                ref var parentHierarchyData = ref hierarchyPool.Get(parent);

                parentHierarchyData.ChildrenCount += 1;
                
                if (parentHierarchyData.FirstChild.IsNull())
                {
                    parentHierarchyData.FirstChild = child;
                    
                    if (!hierarchyPool.Has(child))
                    {
                        ref var childHierarchyData = ref hierarchyPool.Add(child);
                        childHierarchyData.FirstChild = -1;
                        childHierarchyData.NextSibling = -1;
                        childHierarchyData.PrevSibling = -1;
                        childHierarchyData.Parent = parent;
                    }
                    else
                    {
                        ref var childHierarchyData = ref hierarchyPool.Get(child);
                        childHierarchyData.NextSibling = -1;
                        childHierarchyData.PrevSibling = -1;
                        childHierarchyData.Parent = parent;
                    }
                }
                else
                {
                    var prevChild = GetEntityWithHierarchyNullNext(in hierarchyPool, parentHierarchyData.FirstChild);
                    ref var prevChildHierarchyData = ref hierarchyPool.Get(prevChild);
                    prevChildHierarchyData.NextSibling = child;
                    
                    if (!hierarchyPool.Has(child))
                    {
                        ref var childHierarchyData = ref hierarchyPool.Add(child);
                        childHierarchyData.FirstChild = -1;
                        childHierarchyData.NextSibling = -1;
                        childHierarchyData.PrevSibling = prevChild;
                        childHierarchyData.Parent = parent;
                    }
                    else
                    {
                        ref var childHierarchyData = ref hierarchyPool.Get(child);
                        childHierarchyData.NextSibling = -1;
                        childHierarchyData.PrevSibling = prevChild;
                        childHierarchyData.Parent = parent;
                    }
                }
            }

            if (!localToParentPool.Has(child))
            {
                ref var localToParentData = ref localToParentPool.Add(child);
                localToParentData.Position = localTranslation;
            }
            else
            {
                ref var localToParentData = ref localToParentPool.Get(child);
                localToParentData.Position = localTranslation;
            }
        }

        private static int GetEntityWithHierarchyNullNext(in EcsPool<HierarchyData> hierarchyPool, int entity)
        {
            while (true)
            {
                ref var hierarchyData = ref hierarchyPool.Get(entity);
                if (hierarchyData.NextSibling.IsNull())
                {
                    return entity;
                }
                entity = hierarchyData.NextSibling;
            }
        }
    }
}