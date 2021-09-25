using System;
using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class LagCompensationSystem : IEcsRunSystem
    {
        [EcsFilter(typeof(TransformRef), typeof(NetSyncPositionData))]
        private readonly EcsFilter _netSyncPositionGroup = default;

        [EcsPool]
        private readonly EcsPool<TransformRef> _positionPool = default;
        
        [EcsPool]
        private readonly EcsPool<NetSyncPositionData> _netSyncPositionPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _netSyncPositionGroup)
            {
                ref var localToWorld = ref _positionPool.Get(entity);
                ref var netSyncPos = ref _netSyncPositionPool.Get(entity);

                var position = localToWorld.Value.position;
                netSyncPos.OldValue = position;

                position = Vector3.MoveTowards(position, netSyncPos.CurrentValue, Time.deltaTime * netSyncPos.Speed * 10);
                localToWorld.Value.position = position;
            }
        }
    }
}