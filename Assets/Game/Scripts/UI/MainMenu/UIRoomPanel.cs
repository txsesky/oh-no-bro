using System;
using Game.UI.Common;
using Game.UI.MainMenu;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIRoomPanel : UIPanel
    {
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private Transform _playerListContainer;
        [SerializeField] private Button _leaveRoomButton;

        [SerializeField] private GameObject _playerItem;
        
        public Action LeaveRoomButtonAction;

        private void Awake()
        {
            _leaveRoomButton.onClick.AddListener(LeaveRoomButton);
        }

        private void OnDestroy()
        {
            _leaveRoomButton.onClick.RemoveAllListeners();
        }
        
        public void SetRoomName(string roomName)
        {
            _roomNameText.text = roomName;
        }

        private void LeaveRoomButton()
        {
            LeaveRoomButtonAction.Invoke();
        }

        public void InitPlayer(Player player)
        {
            Instantiate(_playerItem, _playerListContainer).GetComponent<PlayerItem>().Setup(player);
        }

        public void RemovePlayer(Player player)
        {
            foreach (Transform child in _playerListContainer)
            {
                var playerItem = child.GetComponent<PlayerItem>();
                if (Equals(player, playerItem.Player))
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
