using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems {
	sealed class TimeSystem : IEcsRunSystem {
		[EcsShared]
		readonly SharedData _sharedData = default;

		public void Run(EcsSystems systems) {
			_sharedData.Time.Time = Time.time;
			_sharedData.Time.DeltaTime = Time.deltaTime;
			_sharedData.Time.SmoothDeltaTime = Time.smoothDeltaTime;
		}
	}
}