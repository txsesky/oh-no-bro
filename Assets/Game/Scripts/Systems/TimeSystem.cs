using Game.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class TimeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _shared;

        public void Init(EcsSystems systems)
        {
            _shared = systems.GetShared<SharedData>();
        }

        public void Run(EcsSystems systems)
        {
            _shared.Time.Time = Time.time;
            _shared.Time.DeltaTime = Time.deltaTime;
            _shared.Time.SmoothDeltaTime = Time.smoothDeltaTime;
        }
    }
}