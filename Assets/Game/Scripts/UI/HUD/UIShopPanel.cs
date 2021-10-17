using System;
using Game.Components;
using Game.UI.Common;
using UnityEngine;

namespace Game.UI.HUD {
	public class UIShopPanel : UIPanel {
		[SerializeField]
		Transform _shopItemElmntContainer;

		public Transform GetItemsContainer() {
			return _shopItemElmntContainer;
		}
	}
}