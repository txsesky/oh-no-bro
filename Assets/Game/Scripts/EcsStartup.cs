using System.Collections.Generic;
using Game.Components;
using Game.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace Game
{
    public class EcsStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystems _physicsSystems;
        private SharedData _sharedData;
    
        private void Start()
        {
            _world = new EcsWorld();
            RuntimeData.EcsWorld = _world;

            _sharedData = new SharedData
            {
                Camera = Camera.main,
                DeltaTime = Time.deltaTime,
                FixedDeltaTime = Time.fixedDeltaTime
            };

            _systems = new EcsSystems(_world, _sharedData);
            _systems
                .Add(new SpawnSystem())
                .Add(new InputGatheringSystem())
                .Add(new TrackDirectionSystem())
                .Add(new MovementSystem())
                .Add(new LagCompensationSystem())
                .Add(new TransformUpdateSystem())
                .Add(new ShopSystem())
                .Add(new DisposeSystem())
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                
                .DelHere<DisposeData>()
                .Inject()
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