using Game.Components;
using Game.Systems;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace Game.Managers
{
    public class ECSManager : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystems _physicsSystems;
        private SharedData _sharedData;
    
        private void Start()
        {
            _world = new EcsWorld();
            EcsPhysicsEvents.ecsWorld = _world;
            
            _sharedData = new SharedData
            {
                Camera = Camera.main, //TODO make in ecs way
                DeltaTime = Time.deltaTime,
                FixedDeltaTime = Time.fixedDeltaTime,
                GameState = new GameState
                {
                    GameMode = GameMode.SinglePlayer
                }
            };

            _systems = new EcsSystems(_world, _sharedData);
            _systems
                .Add(new ParentingSystem())
                .Add(new SpawnSystem())
                .Add(new InputGatheringSystem())
                .Add(new TrackDirectionSystem())
                .Add(new MovementSystem());
            if(_sharedData.GameState.GameMode == GameMode.Client)
                _systems
                    .Add(new LagCompensationSystem());
            _systems
                .Add(new ShopSystem())
                .Add(new DisposeSystem())
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                
                .DelHere<DisposeData>()
                .Inject()
                .DelHerePhysics()
                .Init();

            _physicsSystems = new EcsSystems(_world, _sharedData);
            _physicsSystems
                .Inject()
                .Init();
        }

        private void Update()
        {
            _sharedData.DeltaTime = Time.deltaTime;
            _systems?.Run();
        }

        private void FixedUpdate()
        {
            _sharedData.FixedDeltaTime = Time.fixedDeltaTime;
            _physicsSystems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                EcsPhysicsEvents.ecsWorld = null;
                _systems.Destroy();
                _physicsSystems.Destroy();
                _systems = null;
                _physicsSystems = null;
            }
        
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}