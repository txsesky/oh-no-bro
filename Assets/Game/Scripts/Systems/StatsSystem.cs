using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems {
	sealed class StatsSystem : IEcsRunSystem {
		[EcsFilter(typeof(MovementSpeedData), typeof(PlayerData))]
		readonly EcsFilter _movementSpeedGroup;

		readonly EcsPool<MovementSpeedData> _movementSpeedPool = default;

		public void Run(EcsSystems systems) {
			foreach (var ent in _movementSpeedGroup) {
				ref var movementSpeedData = ref _movementSpeedPool.Get(ent);
				movementSpeedData.Modified = movementSpeedData.Base * movementSpeedData.StatModifier.MultVal +
				                             movementSpeedData.StatModifier.AddVal;
			}
		}
	}
}