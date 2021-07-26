using System;
using Game.Globals;
using Game.Services;
using Game.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using LeopotamGroup.Globals;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Game
{
    sealed class SharedData
    {
        public TimeService Time;
    }
    
    sealed class EcsStartup : MonoBehaviour
    {
        EcsSystems _systems;
        EcsUguiEmitter _ui;

        private void Awake()
        {
            _ui = GameObject.Find("UI").AddComponent<EcsUguiEmitter>();
        }

        void Start()
        {
            var shared = new SharedData
            {
                Time = new TimeService()
            };

            _systems = new EcsSystems(new EcsWorld(), shared);
            _systems
                .AddWorld(new EcsWorld(), Idents.Worlds.Events)
                .AddWorld(new EcsWorld(), Idents.Worlds.UiEvents)
                .Add(new TimeSystem())
                .Add(new MainMenuSystem())
                .InjectUgui(_ui, Idents.Worlds.UiEvents)
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(Idents.Worlds.Events))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(Idents.Worlds.UiEvents))
#endif
                .ConvertScene()
                .Init();
        }

        void Update()
        {
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems.GetWorld(Idents.Worlds.Events).Destroy ();
                _systems.GetWorld(Idents.Worlds.UiEvents).Destroy ();
                _systems.GetWorld().Destroy();
                _systems = null;
            }
        }
    }
}