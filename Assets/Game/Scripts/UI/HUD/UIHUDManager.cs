using System;
using System.Collections.Generic;
using Game.Events;
using Game.UI.Common;
using UnityEngine;

namespace Game.UI.HUD
{
    public class UIHUDManager : UIManager
    {
        [SerializeField] private UIHUDPanel _uiHUDPanel;
        //[SerializeField] private UIShopPanel _uiShopPanel;

        private void Awake()
        {
            Panels = new List<UIPanel>
            {
                _uiHUDPanel,
                //_uiShopPanel
            };
            
            ResetPanels();
            _uiHUDPanel.Show();

            //MessageBus.ShopOpenUIEvent += _uiShopPanel.Show;
            //MessageBus.ShopCloseUIEvent += _uiShopPanel.Hide;
        }

        private void OnDestroy()
        {
            //MessageBus.ShopOpenUIEvent -= _uiShopPanel.Show;
            //MessageBus.ShopCloseUIEvent -= _uiShopPanel.Hide;
        }
    }
}
