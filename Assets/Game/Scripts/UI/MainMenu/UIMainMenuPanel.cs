using System;
using Game.UI.Common;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WebSocketSharp;
using Random = UnityEngine.Random;

namespace Game.UI
{
    public class UIMainMenuPanel : UIPanel
    {
        [SerializeField] private TMP_InputField _playerNickName;
        [SerializeField] private Button _hostGameButton;
        [SerializeField] private Button _joinGameButton;
        [SerializeField] private Button _quitGameButton;

        public Action HostGameButtonAction;
        public Action JoinGameButtonAction;
        public Action QuitGameButtonAction;

        private void Awake()
        {
            _hostGameButton.onClick.AddListener(HostGameButton);
            _joinGameButton.onClick.AddListener(JoinGameButton);
            _quitGameButton.onClick.AddListener(QuitGameButton);
            _playerNickName.onValueChanged.AddListener(ChangeNickName);
        }

        private void OnDestroy()
        {
            _hostGameButton.onClick.RemoveAllListeners();
            _joinGameButton.onClick.RemoveAllListeners();
            _quitGameButton.onClick.RemoveAllListeners();
        }

        private void HostGameButton()
        {
            HostGameButtonAction.Invoke();
        }

        private void JoinGameButton()
        {
            JoinGameButtonAction.Invoke();
        }

        private void QuitGameButton()
        {
            QuitGameButtonAction.Invoke();
            Application.Quit(); //TODO extract to another controller
        }

        private void ChangeNickName(string name)
        {
            PhotonNetwork.NickName = name;
        }
    }
}
