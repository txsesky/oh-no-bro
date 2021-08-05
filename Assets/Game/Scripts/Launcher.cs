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
        private void Awake()
        {
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
            Debug.Log("Connecting to Master");
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            MessageBus.JoinedLobbyEvent?.Invoke();
            Debug.Log("Joined Lobby");
        }

        public void CreateRoom(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return;
            }

            PhotonNetwork.CreateRoom(roomName);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.NickName = RuntimeData.playerNickName.IsNullOrEmpty()
                ? "Player " + Random.Range(0, 10000).ToString("0000")
                : RuntimeData.playerNickName;

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

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("left blya");
            MessageBus.PlayerLeftRoom?.Invoke(otherPlayer);
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