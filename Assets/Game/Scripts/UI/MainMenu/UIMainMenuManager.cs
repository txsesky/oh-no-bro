using System;
using System.Collections.Generic;
using Game.Events;
using Game.UI.Common;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Game.UI {
	public class UIMainMenuManager : UIManager {
		[SerializeField]
		UIMainMenuPanel _uiMainMenuPanel;

		[SerializeField]
		UICreateRoomPanel _uiCreateRoomPanel;

		[SerializeField]
		UIFindRoomPanel _uiFindRoomPanel;

		[SerializeField]
		UIRoomPanel _uiRoomPanel;

		[SerializeField]
		UIErrorPanel _uiErrorPanel;

		[SerializeField]
		UILoadingPanel _uiLoadingPanel;

		void Awake() {
			Panels = new List<UIPanel> {
				_uiMainMenuPanel,
				_uiCreateRoomPanel,
				_uiFindRoomPanel,
				_uiRoomPanel,
				_uiErrorPanel,
				_uiLoadingPanel
			};

			ResetPanels();
			_uiLoadingPanel.Show();

			MessageBus.JoinedLobbyEvent += OpenMainMenuPanel;
			MessageBus.JoinedRoomEvent += OpenRoomPanel;
			MessageBus.CreateRoomFailedEvent += OpenErrorPanel;
			MessageBus.RoomListUpdateEvent += InitRoomList; //_roomListUpdateEvent.OnEventRaised += InitRoomList;
			MessageBus.PlayerEnteredRoomEvent += InitPlayer;
			MessageBus.OtherLeftRoomEvent += RemovePlayer;
			MessageBus.MeLeftRoomEvent += CleanPlayers;
		}

		void OnDestroy() {
			MessageBus.JoinedLobbyEvent -= OpenMainMenuPanel;
			MessageBus.JoinedRoomEvent -= OpenRoomPanel;
			MessageBus.CreateRoomFailedEvent -= OpenErrorPanel;
			MessageBus.RoomListUpdateEvent -= InitRoomList;
			MessageBus.PlayerEnteredRoomEvent -= InitPlayer;
			MessageBus.OtherLeftRoomEvent -= RemovePlayer;
			MessageBus.MeLeftRoomEvent -= CleanPlayers;
		}

		void InitRoomList(List<RoomInfo> roomList) {
			_uiFindRoomPanel.InitRoomList(roomList);
		}

		void InitPlayer(Player player) {
			_uiRoomPanel.InitPlayer(player);
		}

		void RemovePlayer(Player player) {
			_uiRoomPanel.RemovePlayer(player);
		}

		void CleanPlayers() {
			_uiRoomPanel.CleanPlayers();
		}

		void OpenMainMenuPanel() {
			_uiFindRoomPanel.BackButtonAction = delegate { };
			_uiRoomPanel.LeaveRoomButtonAction = delegate { };

			_uiLoadingPanel.Hide();
			_uiMainMenuPanel.Show();

			_uiMainMenuPanel.HostGameButtonAction = OpenCreateRoomPanel;
			_uiMainMenuPanel.JoinGameButtonAction = OpenFindRoomPanel;
		}

		void OpenCreateRoomPanel() {
			_uiMainMenuPanel.HostGameButtonAction = delegate { };

			_uiMainMenuPanel.Hide();
			_uiCreateRoomPanel.Show();

			_uiCreateRoomPanel.CreateRoomAction = CreateRoomButtonClicked;
		}

		void OpenFindRoomPanel() {
			_uiMainMenuPanel.JoinGameButtonAction = delegate { };

			_uiMainMenuPanel.Hide();
			_uiFindRoomPanel.Show();

			_uiFindRoomPanel.JoinRoomAction = JoinRoom;
			_uiFindRoomPanel.BackButtonAction = delegate {
				_uiFindRoomPanel.JoinRoomAction = delegate { };
				_uiFindRoomPanel.Hide();

				OpenMainMenuPanel();
			};
		}

		void JoinRoom(RoomInfo roomInfo) {
			_uiFindRoomPanel.JoinRoomAction = delegate { };
			_uiFindRoomPanel.Hide();

			MessageBus.JoinRoomEvent(roomInfo);
		}

		void OpenRoomPanel(string roomName) {
			_uiLoadingPanel.Hide();

			_uiRoomPanel.Show();
			_uiRoomPanel.SetRoomName(roomName);

			_uiRoomPanel.LeaveRoomButtonAction = delegate {
				MessageBus.LeaveRoomEvent?.Invoke();
				_uiRoomPanel.Hide();
				_uiLoadingPanel.Show();
			};

			if (!PhotonNetwork.IsMasterClient) {
				_uiRoomPanel.StartGameButtonSetActive(false);
			}
			else {
				_uiRoomPanel.StartGameButtonSetActive(true);
				_uiRoomPanel.StartGameButtonAction = delegate { PhotonNetwork.LoadLevel("Game"); };
			}
		}

		void OpenErrorPanel(string message) {
			_uiLoadingPanel.Hide();
			_uiMainMenuPanel.Hide();
			_uiCreateRoomPanel.Hide();
			_uiRoomPanel.Hide();
			_uiErrorPanel.Show();
			_uiErrorPanel.SetErrorMessage(message);

			_uiErrorPanel.BackButtonAction = delegate {
				_uiErrorPanel.Hide();
				OpenMainMenuPanel();
			};
		}

		void CreateRoomButtonClicked(string roomName) {
			_uiCreateRoomPanel.CreateRoomAction = delegate { };

			_uiCreateRoomPanel.Hide();
			_uiLoadingPanel.Show();

			MessageBus.CreateRoomEvent.Invoke(roomName);
		}
	}
}