using Game.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.HUD
{
    public class UIShopItemElmnt : MonoBehaviour
    {
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private Image _itemImage;
        [SerializeField] private Button _itemButton;

        public ShopItemData ShopItemData;
        
        public void Setup(ShopItemData shopItemData)
        {
            ShopItemData = shopItemData;
            _itemName.text = shopItemData.Name;
        }

        public void SetInteractable(bool value)
        {
            _itemButton.interactable = value;
        }

        public Button GetSelectable()
        {
            return _itemButton;
        }
    }
}
