using Game.Services;
using Game.Systems;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Game {
	public class EcsManager : MonoBehaviour {
		EcsSystems _update;
		EcsSystems _fixedUpdate;

		void Awake() {
			var uiEmitter = FindObjectOfType<EcsUguiEmitter>();

			var shared = new SharedData {
				Time = new TimeService(),
				GameState = new GameStateService(Idents.GameModes.SinglePlayer),
				GoogleDocs = new GoogleDocsService()
			};

			_update = new EcsSystems(new EcsWorld(), shared);
			_update
				.AddWorld(new EcsWorld(), Idents.Worlds.Events)
				.AddWorld(new EcsWorld(), Idents.Worlds.UiEvents)
				.Add(new TimeSystem())
				.Add(new CameraSpawnSystem())
				.Add(new SpawnSystem())
				.Add(new ParentingSystem())
				.Add(new InputGatheringSystem())
				.Add(new TrackDirectionSystem())
				.Add(new StatsSystem())
				.Add(new MovementSystem());
			if (shared.GameState.GameMode == Idents.GameModes.Client)
				_update
					.Add(new LagCompensationSystem());
			_update
				.Add(new ShopSystem())
				.Add(new DisposeSystem())
				.InjectUgui(uiEmitter, Idents.Worlds.UiEvents)
				.Inject()
#if UNITY_EDITOR
				.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
				.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(Idents.Worlds.Events))
				.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(Idents.Worlds.UiEvents))
#endif
				.DelHerePhysics()
				.Init();

			EcsPhysicsEvents.ecsWorld = _update.GetWorld();

			_fixedUpdate = new EcsSystems(_update.GetWorld(), shared);
			_fixedUpdate
				.Add(new FixedTimeSystem())
				.Inject()
				.Init();
		}

		void Update() {
			_update?.Run();
		}

		void FixedUpdate() {
			_fixedUpdate?.Run();
		}

		void OnDestroy() {
			if (_update == null) return;
			EcsPhysicsEvents.ecsWorld = null;
			_update.Destroy();
			_fixedUpdate.Destroy();
			_update.GetWorld().Destroy();
			_update.GetWorld(Idents.Worlds.Events).Destroy();
			_update.GetWorld(Idents.Worlds.UiEvents).Destroy();
			_update = null;
			_fixedUpdate = null;
		}
	}
}