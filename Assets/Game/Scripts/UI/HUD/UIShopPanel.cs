using System;
using Game.Components;
using Game.UI.Common;
using UnityEngine;

namespace Game.UI.HUD
{
    public class UIShopPanel : UIPanel
    {
        [SerializeField] private Transform _shopItemElmntContainer;
        //[SerializeField] private GameObject _shopItemElmntPrefab;

        private void Start()
        {
            var movementSpeedItem = new ShopItemData()
            {
                Name = "Movement Speed",
                Cost = 10,
                Stats = new [] {Stat.MS},
                StatsModifiers = new [] {new StatModifier()
                {
                    AddVal = 1
                }}
            };

            //var go = Instantiate(_shopItemElmntPrefab, _shopItemElmntContainer);
            //var c = go.GetComponent<UIShopItemElmnt>();
            //c.Setup(movementSpeedItem);
        }

        public Transform GetItemsContainer()
        {
            return _shopItemElmntContainer;
        }

        private void OnItemButtonAction(UIShopItemElmnt c)
        {
            //try to buy with tradecomponent
            //all above in system
            //c.ItemData.Level += 1;
            //c.ItemData.Cost = get item cost from table (c.ItemData.Name, c.ItemData.Level);
            //for i=0, i>stats.length, i++
            //c.ItemData.StatsModifiers[i] = get item stats from table (c.ItemData.Name, c.ItemData.Stat[i], c.ItemData.Level);
        }
    }
}