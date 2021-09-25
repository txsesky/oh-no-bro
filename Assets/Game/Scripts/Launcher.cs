using System;
using System.Collections.Generic;
using Game.Events;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using WebSocketSharp;
using Random = UnityEngine.Random;

namespace Game
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private string _gameVersion = "1";
        [SerializeField] private byte _maxPlayersPerRoom = 8;
        
        private void Awake()
        {
            _gameVersion = Application.version;
            
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = "Player " + Random.Range(0, 10000).ToString("0000");
            PhotonNetwork.SendRate = 30;
            PhotonNetwork.SerializationRate = 30;
            
            MessageBus.CreateRoomEvent += CreateRoom;
            MessageBus.LeaveRoomEvent += LeaveRoom;
            MessageBus.JoinRoomEvent += JoinRoom;
        }

        private void OnDestroy()
        {
            MessageBus.CreateRoomEvent -= CreateRoom;
            MessageBus.LeaveRoomEvent -= LeaveRoom;
            MessageBus.JoinRoomEvent -= JoinRoom;
        }

        private void Start()
        {
            Connect();
        }

        private void Connect()
        {
            if (PhotonNetwork.IsConnected) return;
            
            Debug.Log("Connecting to Master");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log($"OnConnectedToMaster() was called by PUN ping: {PhotonNetwork.GetPing()}ms");
            PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby() was called by PUN");
            MessageBus.JoinedLobbyEvent?.Invoke();
        }

        public override void OnLeftLobby()
        {
            Debug.Log("OnLeftLobby() was called by PUN");
        }

        public void CreateRoom(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return;
            }

            PhotonNetwork.CreateRoom(roomName, new RoomOptions{MaxPlayers = _maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            MessageBus.JoinedRoomEvent?.Invoke(PhotonNetwork.CurrentRoom.Name);

            var players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                MessageBus.PlayerEnteredRoomEvent?.Invoke(players[i]);
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            MessageBus.CreateRoomFailedEvent?.Invoke($"Room Creation Failed: {message}");
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            MessageBus.MeLeftRoomEvent?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("left blya");
            MessageBus.OtherLeftRoomEvent?.Invoke(otherPlayer);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            MessageBus.RoomListUpdateEvent?.Invoke(roomList); //_roomListUpdateEvent?.Invoke(roomList);
        }

        public void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            MessageBus.PlayerEnteredRoomEvent?.Invoke(newPlayer);
        }
    }
}