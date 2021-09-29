using Game.UI.Common;
using Game.UI.MainMenu;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI.HUD
{
    public class UIHUDPanel : UIPanel
    {
        [SerializeField] private Transform _goldAmountElmntContainer;

        [SerializeField] private GameObject _goldAmountItem;
    
        public void InitPlayer(Player player)
        {
            Instantiate(_goldAmountItem, _goldAmountElmntContainer).GetComponent<UIGoldAmountElmnt>().Setup(player);
        }

        public void RemovePlayer(Player player)
        {
            foreach (Transform child in _goldAmountElmntContainer)
            {
                var playerItem = child.GetComponent<UIPlayerElmnt>();
                if (Equals(player, playerItem.Player))
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public void CleanPlayers()
        {
            foreach (Transform child in _goldAmountElmntContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}