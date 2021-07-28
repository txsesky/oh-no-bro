using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI.MainMenu
{
    public class PlayerItem : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text _playerNameText;
        private Player _player;

        public void Setup(Player player)
        {
            _player = player;
            _playerNameText.text = player.NickName;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (!Equals(_player, otherPlayer))
                return;
            
            Destroy(gameObject);
        }

        public override void OnLeftRoom()
        {
            Destroy(gameObject);
        }
    }
}
