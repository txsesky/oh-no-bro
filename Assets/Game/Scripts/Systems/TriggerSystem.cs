using System;
using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class TriggerSystem : IEcsRunSystem
    {
        [EcsWorld] private readonly EcsWorld _world = default;
        
        [EcsShared] private readonly SharedData _sharedData = default;

        [EcsFilter(typeof(PositionData), typeof(ColliderData), typeof(PlayerData))]
        private readonly EcsFilter _triggerAEntities = default;

        [EcsFilter(typeof(PositionData), typeof(TriggerData))]
        private readonly EcsFilter _triggerBEntities = default;

        [EcsPool] private readonly EcsPool<ColliderData> _colliderPool = default;

        [EcsPool] private readonly EcsPool<PositionData> _positionPool = default;

        [EcsPool] private readonly EcsPool<TriggerData> _triggerPool = default;

        [EcsPool] private readonly EcsPool<OnEnterTriggerEventData> _onEnterTriggerEventPool = default;

        [EcsPool] private readonly EcsPool<OnStayTriggerEventData> _onStayTriggerEventPool = default;

        [EcsPool] private readonly EcsPool<OnExitTriggerEventData> _onExitTriggerEventPool = default;

        public void Run(EcsSystems systems)
        {
            foreach (var triggerAEntity in _triggerAEntities)
            {
                ref var positionA = ref _positionPool.Get(triggerAEntity);
                //ref var colliderA = ref _colliderPool.Get(colliderEntity);
                ref var triggerA = ref _triggerPool.Get(triggerAEntity);


                foreach (var triggerBEntity in _triggerBEntities)
                {
                    ref var positionB = ref _positionPool.Get(triggerBEntity);
                    ref var triggerB = ref _triggerPool.Get(triggerBEntity);

                    if (AreOverlapping(triggerA.Radius, triggerB.Radius, positionA.Value, positionB.Value))
                    {
                        foreach (var triggerEventSystem in _sharedData.TriggerEventSystems)
                        {
                            triggerEventSystem.Trigger(triggerAEntity, triggerBEntity);
                        }
                        //new
                        //var hasStay = false;

                        //foreach (var entity in _onEnterTriggerEventPool)
                        //{
                        //    
                        //}

                        //if (!hasStay)
                        //{
                        //    var onEnterEventEntity = _world.NewEntity();
                        //    ref var onEnterEventData = ref _onEnterTriggerEventPool.Add(onEnterEventEntity);
                        //    onEnterEventData.TriggerEntity = triggerEntity;
                        //    onEnterEventData.ColliderEntity = colliderEntity;
                        
                        //    var onStayEventEntity = _world.NewEntity();
                        //    ref var onStayEventData = ref _onStayTriggerEventPool.Add(onStayEventEntity);
                        //    onStayEventData.TriggerEntity = triggerEntity;
                        //    onStayEventData.ColliderEntity = colliderEntity;
                        //} 
                        //old
                        //if (_onStayTriggerEventPool.Has(colliderEntity))
                        //    continue;
                        //
                        //ref var onEnterTriggerEventData = ref _onEnterTriggerEventPool.Add(colliderEntity);
                        //onEnterTriggerEventData.TriggerEntity = triggerEntity;
                        //onEnterTriggerEventData.TriggerEntity = triggerEntity;
                        //
                        //ref var onStayTriggerEventData = ref _onStayTriggerEventPool.Add(colliderEntity);
                        //onStayTriggerEventData.TriggerEntity = triggerEntity;
                    }

                    if (AreSeparate(triggerA.Radius, triggerB.Radius, positionA.Value, positionB.Value))
                    {
                        //if (!_onStayTriggerEventPool.Has(colliderEntity))
                        //    continue;
                        //
                        //_onStayTriggerEventPool.Del(colliderEntity);
                        //
                        //ref var onExitTriggerEventData = ref _onExitTriggerEventPool.Add(colliderEntity);
                        //onExitTriggerEventData.TriggerEntity = triggerEntity;
                    }
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

        private bool AreSeparate(float radiusA, float radiusB, Vector2 posA, Vector2 posB)
        {
            var dx = posA.x - posB.x;
            var dy = posA.y - posB.y;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            return distance > radiusA + radiusB;
        }
    }
}