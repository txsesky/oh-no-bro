using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI.MainMenu
{
    public class RoomItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private Button _roomButton;

        private RoomInfo _info;

        public UnityAction<RoomInfo, RoomItem> RoomButtonAction;

        public void Setup(RoomInfo info)
        {
            _info = info;
            _roomNameText.text = info.Name;
            _roomButton.onClick.AddListener(delegate
            {
                RoomButton(info);
            });
        }

        public void RoomButton(RoomInfo info)
        {
            RoomButtonAction.Invoke(info, this);
        }
    }
}
