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
        [SerializeField] private Button _startGameButton;

        [SerializeField] private GameObject _playerItem;
        
        public Action LeaveRoomButtonAction;
        public Action StartGameButtonAction;

        private void Awake()
        {
            _leaveRoomButton.onClick.AddListener(LeaveRoomButton);
            _startGameButton.onClick.AddListener(StartGameButton);
        }

        private void OnDestroy()
        {
            _leaveRoomButton.onClick.RemoveAllListeners();
            _startGameButton.onClick.RemoveAllListeners();
        }
        
        public void SetRoomName(string roomName)
        {
            _roomNameText.text = roomName;
        }

        private void LeaveRoomButton()
        {
            LeaveRoomButtonAction.Invoke();
        }
        
        private void StartGameButton()
        {
            StartGameButtonAction.Invoke();
        }

        public void InitPlayer(Player player)
        {
            Instantiate(_playerItem, _playerListContainer).GetComponent<UIPlayerElmnt>().Setup(player);
        }

        public void RemovePlayer(Player player)
        {
            foreach (Transform child in _playerListContainer)
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
            foreach (Transform child in _playerListContainer)
            {
                Destroy(child.gameObject);
            }
        }

        public void StartGameButtonSetActive(bool value)
        {
            _startGameButton.gameObject.SetActive(value);
        }
    }
}
