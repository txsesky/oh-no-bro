using System;
using Game.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class UICreateRoomPanel : UIPanel
    {
        [SerializeField] private TMP_InputField _roomNameInput;
        [SerializeField] private Button _createRoomButton;
        
        public Action<string> CreateRoomAction;

        private void Awake()
        {
            _createRoomButton.onClick.AddListener(CreateRoomButton);
        }

        private void OnDestroy()
        {
            _createRoomButton.onClick.RemoveAllListeners();
        }

        private void CreateRoomButton()
        {
            CreateRoomAction.Invoke(_roomNameInput.text);
        }
    }
}