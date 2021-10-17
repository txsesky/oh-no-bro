using Game.Components;
using Game.Services;
using Game.Systems;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;

namespace Game {
	public class EcsManager : MonoBehaviour {
		EcsSystems _update;
		EcsSystems _fixedUpdate;

		void Awake() {
			var uiEmitter = FindObjectOfType<EcsUguiEmitter>();

			var time = new TimeService();
			var gameState = new GameStateService(Idents.GameModes.SinglePlayer);
			var googleDocs = new GoogleDocsService();

			var shared = new SharedData {
				Time = time,
				GameState = gameState,
				GoogleDocs = googleDocs
			};

			_update = new EcsSystems(new EcsWorld(), shared);
			_update
				.Add(new TimeSystem())
				.Add(new CameraSpawnSystem())
				.Add(new CreateItemsSystem())
				.Add(new SinglePlayerSpawnSystem())
				.Add(new PlayersSpawnSystem())
				.Add(new EnvironmentLoadSystem())
				.Add(new UILoadSystem())
				.Add(new UIShopSystem())
				.Add(new ParentingSystem())
				.Add(new BuyCooldownSystem())
				.Add(new AddModifierSystem())
				.Add(new InputGatheringSystem())
				.Add(new TrackDirectionSystem())
				.Add(new MovementSystem())
				.Add(new LagCompensationSystem())
				.Add(new ShopSystem())
				.Add(new DisposeSystem())
				.InjectUgui(uiEmitter)
				.Inject()
#if UNITY_EDITOR
				.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
				.DelHere<BuyItemEvent>()
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
			_update = null;
			_fixedUpdate = null;
		}
	}
}