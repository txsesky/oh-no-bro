using System.Collections;
using System.Collections.Generic;
using Game.UI.Common;
using Game.UI.MainMenu;
using Photon.Realtime;
using UnityEngine;

public class UIMainGameplayPanel : UIPanel
{
    [SerializeField] private Transform _goldAmountListContainer;

    [SerializeField] private GameObject _goldAmountItem;
    
    public void InitPlayer(Player player)
    {
        Instantiate(_goldAmountItem, _goldAmountListContainer).GetComponent<GoldAmountItem>().Setup(player);
    }

    public void RemovePlayer(Player player)
    {
        foreach (Transform child in _goldAmountListContainer)
        {
            var playerItem = child.GetComponent<PlayerItem>();
            if (Equals(player, playerItem.Player))
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void CleanPlayers()
    {
        foreach (Transform child in _goldAmountListContainer)
        {
            Destroy(child.gameObject);
        }
    }
}