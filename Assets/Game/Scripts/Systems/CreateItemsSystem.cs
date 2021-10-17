using Game.Components;
using Game.Factory;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems {
	sealed class CreateItemsSystem : IEcsInitSystem {
		readonly EcsWorld _world = default;

		public void Init(EcsSystems systems) {
			//foreach (var VARIABLE in COLLECTION) {
				var e = _world.NewEntity();
				
				ItemFactory.CreateItem(e, _world);
			//}
		}
	}
}