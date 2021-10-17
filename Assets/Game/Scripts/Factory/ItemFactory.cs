using Game.Components;
using Leopotam.EcsLite;

namespace Game.Factory {
	public class ItemFactory {
		public static void CreateItem(int entity, EcsWorld ecsWorld) {
			var itemPool = ecsWorld.GetPool<ItemComponent>();

			ref var item = ref itemPool.Add(entity);
		}
	}
}