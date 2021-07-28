using System;
using Photon.Pun;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Game
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameEvent _joinedLobbyEvent_Channel;
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
            _joinedLobbyEvent_Channel.Raise();
            Debug.Log("Joined Lobby");
        }

        private void Update()
        {
            
        }
    }
}