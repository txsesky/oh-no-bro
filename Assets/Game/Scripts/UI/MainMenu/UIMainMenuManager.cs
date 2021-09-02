using System;
using System.Collections.Generic;
using Game.Events;
using Photon.Pun;
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
            MessageBus.OtherLeftRoomEvent += RemovePlayer;
            MessageBus.MeLeftRoomEvent += CleanPlayers;
        }

        private void OnDestroy()
        {
            MessageBus.JoinedLobbyEvent -= OpenMainMenuPanel;
            MessageBus.JoinedRoomEvent -= OpenRoomPanel;
            MessageBus.CreateRoomFailedEvent -= OpenErrorPanel;
            MessageBus.RoomListUpdateEvent -= InitRoomList;
            MessageBus.PlayerEnteredRoomEvent -= InitPlayer;
            MessageBus.OtherLeftRoomEvent -= RemovePlayer;
            MessageBus.MeLeftRoomEvent -= CleanPlayers;
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

        private void CleanPlayers()
        {
            _uiRoomPanel.CleanPlayers();
        }

        private void OpenMainMenuPanel()
        {
            _uiFindRoomPanel.BackButtonAction = delegate { };
            _uiRoomPanel.LeaveRoomButtonAction = delegate { };

            _uiLoadingPanel.Hide();
            _uiMainMenuPanel.Show();
            
            _uiMainMenuPanel.HostGameButtonAction = OpenCreateRoomPanel;
            _uiMainMenuPanel.JoinGameButtonAction = OpenFindRoomPanel;
            
        }

        private void OpenCreateRoomPanel()
        {
            _uiMainMenuPanel.HostGameButtonAction = delegate { };
            
            _uiMainMenuPanel.Hide();
            _uiCreateRoomPanel.Show();
            
            _uiCreateRoomPanel.CreateRoomAction = CreateRoomButtonClicked;
        }

        private void OpenFindRoomPanel()
        {
            _uiMainMenuPanel.JoinGameButtonAction = delegate { };
            
            _uiMainMenuPanel.Hide();
            _uiFindRoomPanel.Show();

            _uiFindRoomPanel.JoinRoomAction = JoinRoom;
            _uiFindRoomPanel.BackButtonAction = delegate
            {
                _uiFindRoomPanel.JoinRoomAction = delegate {  };
                _uiFindRoomPanel.Hide();
                
                OpenMainMenuPanel();
            };
        }

        private void JoinRoom(RoomInfo roomInfo)
        {
            _uiFindRoomPanel.JoinRoomAction = delegate {  };
            _uiFindRoomPanel.Hide();
            
            MessageBus.JoinRoomEvent(roomInfo);
        }

        private void OpenRoomPanel(string roomName)
        {
            _uiLoadingPanel.Hide();
            
            _uiRoomPanel.Show();
            _uiRoomPanel.SetRoomName(roomName);
            
            _uiRoomPanel.LeaveRoomButtonAction = delegate
            {
                MessageBus.LeaveRoomEvent?.Invoke();
                _uiRoomPanel.Hide();
                _uiLoadingPanel.Show();
            };
            
            if (!PhotonNetwork.IsMasterClient)
            {
                _uiRoomPanel.StartGameButtonSetActive(false);
            }
            else
            {
                _uiRoomPanel.StartGameButtonSetActive(true);
                _uiRoomPanel.StartGameButtonAction = delegate
                {
                    PhotonNetwork.LoadLevel("Game");
                };
            }
        }

        private void OpenErrorPanel(string message)
        {
            _uiLoadingPanel.Hide();
            _uiMainMenuPanel.Hide();
            _uiCreateRoomPanel.Hide();
            _uiRoomPanel.Hide();
            _uiErrorPanel.Show();
            _uiErrorPanel.SetErrorMessage(message);
            
            _uiErrorPanel.BackButtonAction = delegate
            {
                _uiErrorPanel.Hide();
                OpenMainMenuPanel();
            };
        }

        private void CreateRoomButtonClicked(string roomName)
        {
            _uiCreateRoomPanel.CreateRoomAction = delegate { };
            
            _uiCreateRoomPanel.Hide();
            _uiLoadingPanel.Show();
            
            MessageBus.CreateRoomEvent.Invoke(roomName);
        }
    }
}