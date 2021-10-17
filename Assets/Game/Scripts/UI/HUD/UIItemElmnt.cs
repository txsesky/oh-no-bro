using System;
using Game.Components;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.HUD {
	public class UIItemElmnt : MonoBehaviour {
		[SerializeField]
		TMP_Text _itemName;

		[SerializeField]
		Image _itemImage;

		[SerializeField]
		Button _itemButton;
		
		[SerializeField]
		Image _radialProgressImage;

		int _itemEntity;
		EcsWorld _ecsWorld;

		public void Setup(int itemEntity, EcsWorld ecsWorld) {
			var namePool = ecsWorld.GetPool<NameComponent>();
			ref var nameC = ref namePool.Get(itemEntity);
			
			_itemName.text = nameC.Value;
			_itemEntity = itemEntity;
			_ecsWorld = ecsWorld;
			_itemButton.onClick.AddListener(ItemButton);
		}

		public void SetInteractable(bool value) {
			_itemButton.interactable = value;
			_radialProgressImage.gameObject.SetActive(!value);
			if (value)
				FillRadialProgressImage(1);
		}

		public void FillRadialProgressImage(float value) {
			_radialProgressImage.fillAmount = value;
		}

		void ItemButton() {
			var e = _ecsWorld.NewEntity();
			var pool = _ecsWorld.GetPool<BuyItemEvent>();
			ref var buyItemEvent = ref pool.Add(e);
			buyItemEvent.SenderEntity = _itemEntity;
		}

		void OnDestroy() {
			_itemButton.onClick.RemoveAllListeners();
		}
	}
}