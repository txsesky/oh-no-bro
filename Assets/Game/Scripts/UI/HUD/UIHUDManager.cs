using System;
using System.Collections.Generic;
using Game.Events;
using Game.UI.Common;
using UnityEngine;

namespace Game.UI.HUD {
	public class UIHUDManager : UIManager {
		[SerializeField]
		UIHUDPanel _uiHUDPanel;

		void Awake() {
			Panels = new List<UIPanel> {
				_uiHUDPanel,
			};

			ResetPanels();
			_uiHUDPanel.Show();
		}
	}
}