using System;
using System.Collections.Generic;
using Game.Events;
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

        private void Awake()
        {
            ResetPanels();
            
            MessageBus.JoinedLobbyEvent += OpenMainMenuPanel;
            MessageBus.JoinedRoomEvent += OpenRoomPanel;
            MessageBus.CreateRoomFailedEvent += OpenErrorPanel;
            MessageBus.RoomListUpdateEvent += InitRoomList;//_roomListUpdateEvent.OnEventRaised += InitRoomList;
            MessageBus.PlayerEnteredRoomEvent += InitPlayer;
            MessageBus.PlayerLeftRoom += RemovePlayer;
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

        private void RemovePlayer(Player player)
        {
            _uiRoomPanel.RemovePlayer(player);
        }

        private void OnDestroy()
        {
            MessageBus.JoinedLobbyEvent -= OpenMainMenuPanel;
            MessageBus.JoinedRoomEvent -= OpenRoomPanel;
            MessageBus.CreateRoomFailedEvent -= OpenErrorPanel;
            MessageBus.RoomListUpdateEvent -= InitRoomList;
            MessageBus.PlayerEnteredRoomEvent -= InitPlayer;
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
            
            MessageBus.JoinRoomEvent(roomInfo);
        }

        private void OpenRoomPanel(string roomName)
        {
            _uiLoadingPanel.Hide();
            _uiRoomPanel.Show();
            _uiRoomPanel.SetRoomName(roomName);
            
            _uiRoomPanel.LeaveRoomButtonAction += delegate
            {
                MessageBus.LeaveRoomEvent?.Invoke();
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
            
            MessageBus.CreateRoomEvent.Invoke(roomName);
        }
    }
}