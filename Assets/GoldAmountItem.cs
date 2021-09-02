using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Simple;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GoldAmountItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _goldAmountText;

    public Player Player;
    public GameObject Character;

    public void Setup(Player player)
    {
        Player = player;
        Character = player.TagObject as GameObject;

        if (Character is null) return;
        
        var vitals = Character.GetComponent<SyncVitals>();
    }

    public void UpdateText(string playerNameText, string goldAmountText)
    {
        _playerNameText.text = playerNameText;
        _goldAmountText.text = goldAmountText;
    }
}