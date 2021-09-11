using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class TrackDirectionSystem : IEcsRunSystem
    {
        [EcsShared] private readonly SharedData _sharedData = default;

        [EcsFilter(typeof(PositionData), typeof(MovementData), typeof(PlayerData), typeof(InputData))]
        private readonly EcsFilter _playerFilter = default;

        [EcsPool] private readonly EcsPool<InputData> _inputPool = default;

        [EcsPool] private readonly EcsPool<PositionData> _positionPool = default;

        [EcsPool] private readonly EcsPool<MovementData> _movementPool = default;

        public void Run(EcsSystems systems)
        {
            foreach (var playerEntity in _playerFilter)
            {
                ref var input = ref _inputPool.Get(playerEntity);
                ref var position = ref _positionPool.Get(playerEntity);
                ref var movement = ref _movementPool.Get(playerEntity);

                if (!input.IsPerformedToMove)
                {
                    movement.Direction = Vector2.zero;
                    continue;
                }
                
                var plane = new Plane(Vector3.up, Vector3.zero);
                var ray = _sharedData.Camera.ScreenPointToRay(input.PointerPosition);

                if (plane.Raycast(ray, out var point))
                {
                    var destination = ray.GetPoint(point);
                    var posV3 = position.Value;
                    if ((destination - posV3).magnitude > 0.1f)
                    {
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