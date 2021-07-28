using System;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

namespace Game.UI
{
    public class UIMainMenuManager : MonoBehaviour
    {
        [SerializeField] private UIMainMenuPanel _uiMainMenuPanel;
        [SerializeField] private UICreateRoomPanel _uiCreateRoomPanel;
        [SerializeField] private UIFindRoomPanel _uiFindRoomPanel;
        [SerializeField] private UIRoomPanel _uiRoomPanel;
        [SerializeField] private UIErrorPanel _uiErrorPanel;
        [SerializeField] private UILoadingPanel _uiLoadingPanel;
        
        [Header("Listen")]
        [SerializeField] private VoidEventChannelSO _joinedLobbyEvent;
        [SerializeField] private StringEventChannelSO _joinedRoomEvent;
        [SerializeField] private StringEventChannelSO _createRoomFailedEvent;
        [SerializeField] private RoomInfoListEventChannelSO _roomListUpdateEvent;
        [SerializeField] private PlayerInfoEventChannelSO _playerEnteredRoomEvent;

        [Header("Broadcast")]
        [SerializeField] private StringEventChannelSO _createRoomEvent;
        [SerializeField] private VoidEventChannelSO _leaveRoomEvent;
        [SerializeField] private RoomInfoEventChannelSO _joinRoomEvent;

        private void Awake()
        {
            ResetPanels();
            
            _joinedLobbyEvent.OnEventRaised += OpenMainMenuPanel;
            _joinedRoomEvent.OnEventRaised += OpenRoomPanel;
            _createRoomFailedEvent.OnEventRaised += OpenErrorPanel;
            _roomListUpdateEvent.OnEventRaised += InitRoomList;
            _playerEnteredRoomEvent.OnEventRaised += InitPlayer;
        }

        private void ResetPanels()
        {
            _uiMainMenuPanel.Hide();
            _uiCreateRoomPanel.Hide();
            _uiFindRoomPanel.Hide();
            _uiRoomPanel.Hide();
            _uiErrorPanel.Hide();
            _uiLoadingPanel.Show();
        }

        private void InitRoomList(List<RoomInfo> roomList)
        {
            _uiFindRoomPanel.InitRoomList(roomList);
        }
        private void InitPlayer(Player player)
        {
            _uiRoomPanel.InitPlayer(player);
        }

        private void OnDestroy()
        {
            _joinedLobbyEvent.OnEventRaised -= OpenMainMenuPanel;
            _joinedRoomEvent.OnEventRaised -= OpenRoomPanel;
            _createRoomFailedEvent.OnEventRaised -= OpenErrorPanel;
            _roomListUpdateEvent.OnEventRaised -= InitRoomList;
            _playerEnteredRoomEvent.OnEventRaised -= InitPlayer;
        }

        private void OpenMainMenuPanel()
        {
            _uiLoadingPanel.Hide();
            _uiMainMenuPanel.Show();
            
            _uiMainMenuPanel.HostGameButtonAction += OpenCreateRoomPanel;
            _uiMainMenuPanel.JoinGameButtonAction += OpenFindRoomPanel;
        }

        private void OpenCreateRoomPanel()
        {
            _uiMainMenuPanel.HostGameButtonAction -= OpenCreateRoomPanel;
            
            _uiMainMenuPanel.Hide();
            _uiCreateRoomPanel.Show();
            
            _uiCreateRoomPanel.CreateRoomAction += CreateRoomButtonClicked;
        }

        private void OpenFindRoomPanel()
        {
            _uiMainMenuPanel.JoinGameButtonAction -= OpenFindRoomPanel;
            
            _uiMainMenuPanel.Hide();
            _uiFindRoomPanel.Show();

            _uiFindRoomPanel.JoinRoomAction += JoinRoom;
            _uiFindRoomPanel.BackButtonAction += delegate
            {
                _uiFindRoomPanel.JoinRoomAction -= JoinRoom;
                _uiFindRoomPanel.Hide();
                
                OpenMainMenuPanel();
            };
        }

        private void JoinRoom(RoomInfo roomInfo)
        {
            _uiFindRoomPanel.JoinRoomAction -= JoinRoom;
            _uiFindRoomPanel.Hide();
            
            _joinRoomEvent.RaiseEvent(roomInfo);
        }

        private void OpenRoomPanel(string roomName)
        {
            _uiLoadingPanel.Hide();
            _uiRoomPanel.Show();
            _uiRoomPanel.SetRoomName(roomName);
            
            _uiRoomPanel.LeaveRoomButtonAction += delegate
            {
                _leaveRoomEvent.RaiseEvent();
                _uiRoomPanel.Hide();
                _uiLoadingPanel.Show();
            };
        }

        private void OpenErrorPanel(string message)
        {
            _uiLoadingPanel.Hide();
            _uiMainMenuPanel.Hide();
            _uiCreateRoomPanel.Hide();
            _uiRoomPanel.Hide();
            _uiErrorPanel.Show();
            _uiErrorPanel.SetErrorMessage(message);
            
            _uiErrorPanel.BackButtonAction += delegate
            {
                _uiErrorPanel.Hide();
                OpenMainMenuPanel();
            };
        }

        private void CreateRoomButtonClicked(string roomName)
        {
            _uiCreateRoomPanel.CreateRoomAction -= CreateRoomButtonClicked;
            
            _uiCreateRoomPanel.Hide();
            _uiLoadingPanel.Show();
            
            _createRoomEvent.RaiseEvent(roomName);
        }
    }
}