using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems {
	sealed class FixedTimeSystem : IEcsRunSystem {
		[EcsShared] 
		readonly SharedData _sharedData = default;

		public void Run(EcsSystems systems) {
			_sharedData.Time.FixedDeltaTime = Time.deltaTime;
		}
	}
}