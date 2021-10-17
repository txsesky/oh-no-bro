using Game.Components;
using Game.UI.HUD;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems {
	sealed class UILoadSystem : IEcsInitSystem {
		string _shopPanelPrefab = "Prefabs/UI/Shop_Panel";
		
		readonly EcsWorld _world = default;
		readonly EcsPool<TransformRefData> _transformPool = default;
		readonly EcsPool<UIShopPanelRef> _uiShopPanelPool = default;

		public void Init(EcsSystems systems) {
			var e = _world.NewEntity();

			var root = GameObject.Find("UI");
			var go = Object.Instantiate(Resources.Load(_shopPanelPrefab), root.transform) as GameObject;

			if (go == null)
				return;

			ref var transformData = ref _transformPool.Add(e);
			transformData.Value = go.transform;

			ref var shopPanelData = ref _uiShopPanelPool.Add(e);
			shopPanelData.Value = go.GetComponent<UIShopPanel>();
			shopPanelData.Value.Hide();
		}
	}
}