using System;
using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class PositionLagCompensationSystem : IEcsRunSystem
    {
        [EcsFilter(typeof(PositionData), typeof(NetSyncPositionData))]
        private readonly EcsFilter _entities = default;

        [EcsPool]
        private readonly EcsPool<PositionData> _positionPool = default;
        
        [EcsPool]
        private readonly EcsPool<NetSyncPositionData> _netSyncPositionPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _entities)
            {
                ref var position = ref _positionPool.Get(entity);
                ref var netSyncPos = ref _netSyncPositionPool.Get(entity);

                netSyncPos.OldValue = position.Value;

                position.Value = Vector3.MoveTowards(position.Value, netSyncPos.CurrentValue, Time.deltaTime * netSyncPos.Speed * 10);
            }
        }
    }
}