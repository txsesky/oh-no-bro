using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI.MainMenu
{
    public class UIPlayerElmnt : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerNameText;
        public Player Player;

        public void Setup(Player player)
        {
            Player = player;
            _playerNameText.text = player.NickName;
        }
    }
}
