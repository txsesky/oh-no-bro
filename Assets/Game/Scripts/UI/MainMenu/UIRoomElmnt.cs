using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI.MainMenu
{
    public class UIRoomElmnt : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private Button _roomButton;

        private RoomInfo _info;

        public Action<RoomInfo, UIRoomElmnt> RoomButtonAction;

        public void Setup(RoomInfo info)
        {
            _info = info;
            _roomNameText.text = info.Name;
            _roomButton.onClick.AddListener(() => RoomButton(info));
        }

        private void RoomButton(RoomInfo info)
        {
            RoomButtonAction.Invoke(info, this);
        }
    }
}
