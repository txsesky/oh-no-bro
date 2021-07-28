using System;
using System.Collections.Generic;
using Game.UI.Common;
using Game.UI.MainMenu;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIFindRoomPanel : UIPanel
    {
        [SerializeField] private Transform _roomListContainer;
        [SerializeField] private Button _backButton;

        [SerializeField] private GameObject _roomListItemPrefab;
        
        public UnityAction<RoomInfo> JoinRoomAction;
        public UnityAction BackButtonAction;

        private void Awake()
        {
            _backButton.onClick.AddListener(BackButton);
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveAllListeners();
        }

        public void InitRoomList(List<RoomInfo> roomList)
        {
            foreach (var roomInfo in roomList)
            {
                var go = Instantiate(_roomListItemPrefab, _roomListContainer);
                var c = go.GetComponent<RoomItem>();
                c.Setup(roomInfo);
                c.RoomButtonAction += JoinRoom;
            }
        }

        public void JoinRoom(RoomInfo roomInfo, RoomItem roomItem)
        {
            roomItem.RoomButtonAction -= JoinRoom; // разобраться
            JoinRoomAction.Invoke(roomInfo);
        }
        
        public void BackButton()
        {
            BackButtonAction?.Invoke();
        }
    }
}