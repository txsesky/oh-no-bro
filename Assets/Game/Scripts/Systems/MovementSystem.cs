using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems {
	sealed class MovementSystem : IEcsRunSystem {
		[EcsShared]
		readonly SharedData _sharedData = default;

		[EcsFilter(typeof(CharacterControllerRefData), typeof(MovementDirectionData), typeof(MovementSpeedData),
			typeof(PlayerData))]
		readonly EcsFilter _movingEntityGroup = default;

		readonly EcsPool<CharacterControllerRefData> _characterControllerPool = default;
		readonly EcsPool<MovementDirectionData> _movementDirectionPool = default;
		readonly EcsPool<MovementSpeedData> _movementSpeedPool = default;

		public void Run(EcsSystems systems) {
			foreach (var movingEntity in _movingEntityGroup) {
				ref var charContrData = ref _characterControllerPool.Get(movingEntity);
				ref var movementDirectionData = ref _movementDirectionPool.Get(movingEntity);
				ref var movementSpeedData = ref _movementSpeedPool.Get(movingEntity);

				if (movementDirectionData.Direction.x == 0f && movementDirectionData.Direction.y == 0f)
					continue;

				charContrData.Value.Move(
					new Vector3(movementDirectionData.Direction.x, 0, movementDirectionData.Direction.y) *
					movementSpeedData.ValueMeterPerSec * _sharedData.Time.DeltaTime);
			}
		}
	}
}