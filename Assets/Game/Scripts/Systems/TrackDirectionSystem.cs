using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems {
	sealed class TrackDirectionSystem : IEcsRunSystem {
		[EcsFilter(typeof(TransformRefData), typeof(MovementDirectionData), typeof(InputData))]
		readonly EcsFilter _playerGroup = default;
		
		[EcsFilter(typeof(WorldCameraTag))]
		readonly EcsFilter _worldCameraEntity = default;

		readonly EcsPool<InputData> _inputPool = default;
		readonly EcsPool<TransformRefData> _positionPool = default;
		readonly EcsPool<MovementDirectionData> _movementPool = default;
		readonly EcsPool<CameraRefData> _cameraPool = default;

		public void Run(EcsSystems systems) {
			foreach (var playerEntity in _playerGroup) {
				ref var input = ref _inputPool.Get(playerEntity);
				ref var localToWorld = ref _positionPool.Get(playerEntity);
				ref var movement = ref _movementPool.Get(playerEntity);
				ref var worldCamera = ref _cameraPool.Get(_worldCameraEntity.GetRawEntities()[0]);

				if (!input.IsPerformedToMove) {
					movement.Direction = Vector2.zero;
					continue;
				}

				var plane = new Plane(Vector3.up, Vector3.zero);
				var ray = worldCamera.Value.ScreenPointToRay(input.PointerPosition);

				if (plane.Raycast(ray, out var point)) {
					var destination = ray.GetPoint(point);
					var posV3 = localToWorld.Value.position;
					if ((destination - posV3).magnitude > 0.1f) {
						var dir = (destination - posV3).normalized;
						movement.Direction = new Vector2(dir.x, dir.z);
					}
					else
						movement.Direction = Vector2.zero;
				}
			}
		}
	}
}