using System;
using System.Collections.Generic;
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
        [Header("SharedVariable")] 
        [SerializeField] private StringVariable playerNickName;
        
        [Header("Listening on channels")] 
        [SerializeField] private StringEventChannelSO _createRoomEvent;
        [SerializeField] private VoidEventChannelSO _leaveRoomEvent;
        [SerializeField] private RoomInfoEventChannelSO _joinRoomEvent;

        [Header("Broadcasting on channels")] 
        [SerializeField] private VoidEventChannelSO _joinedLobbyEvent;
        [SerializeField] private StringEventChannelSO _joinedRoomEvent;
        [SerializeField] private StringEventChannelSO _createRoomFailedEvent;
        [SerializeField] private RoomInfoListEventChannelSO _roomListUpdateEvent;
        [SerializeField] private PlayerInfoEventChannelSO _playerEnteredRoomEvent;


        private void Awake()
        {
            _createRoomEvent.OnEventRaised += CreateRoom;
            _leaveRoomEvent.OnEventRaised += LeaveRoom;
            _joinRoomEvent.OnEventRaised += JoinRoom;
        }

        private void OnDestroy()
        {
            _createRoomEvent.OnEventRaised -= CreateRoom;
            _leaveRoomEvent.OnEventRaised -= LeaveRoom;
            _joinRoomEvent.OnEventRaised -= JoinRoom;
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
            _joinedLobbyEvent.RaiseEvent();
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
            PhotonNetwork.NickName = playerNickName.RuntimeValue.IsNullOrEmpty() ? "Player " + Random.Range(0, 10000).ToString("0000") : playerNickName.RuntimeValue;
            
            _joinedRoomEvent.RaiseEvent(PhotonNetwork.CurrentRoom.Name);
            
            var players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                _playerEnteredRoomEvent.RaiseEvent(players[i]);
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            _createRoomFailedEvent.RaiseEvent($"Room Creation Failed: {message}");
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

/*
        public override void OnLeftRoom()
        {
            _joinedLobbyEvent.RaiseEvent();
        }
*/
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _roomListUpdateEvent.RaiseEvent(roomList);
        }

        public void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _playerEnteredRoomEvent.RaiseEvent(newPlayer);
        }
    }
}