using Game.Components;
using Leopotam.EcsLite;

namespace Game.Systems {
	sealed class UIShopSystem : IEcsInitSystem, IEcsRunSystem {
		string _shopItemElmntPrefab = "Prefabs/UI/ShopItemElmnt";

		readonly EcsWorld _world = default;
		readonly EcsPool<TransformRefData> _transformPool = default;
		readonly EcsPool<UIShopPanelRef> _uiShopPanelPool = default;
		readonly EcsPool<UItemElmntRef> _uiShopItemElmntPool = default;

		public void Init(EcsSystems systems) {
			/*ref var shopPanelData = ref _uiShopPanelPool.Get(e);
			var root = shopPanelData.Value.GetItemsContainer();

			foreach (var VARIABLE in COLLECTION) {
				var go = Object.Instantiate(Resources.Load(_shopItemElmntPrefab), root) as GameObject;

				if (go == null)
					return;

				ref var itemElmntData = ref _uiShopItemElmntPool.Add(itemEntity);
				itemElmntData.Value = go.GetComponent<UIItemElmnt>();
				itemElmntData.Value.Setup(itemEntity, _world);
			}*/
		}

		public void Run(EcsSystems systems) {
			
		}
	}
}