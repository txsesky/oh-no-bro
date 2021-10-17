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
        [SerializeField]
        TMP_InputField _playerNickName;
        [SerializeField]
        Button _hostGameButton;
        [SerializeField]
        Button _joinGameButton;
        [SerializeField]
        Button _quitGameButton;

        public Action HostGameButtonAction;
        public Action JoinGameButtonAction;
        public Action QuitGameButtonAction;

        void Awake()
        {
            _hostGameButton.onClick.AddListener(HostGameButton);
            _joinGameButton.onClick.AddListener(JoinGameButton);
            _quitGameButton.onClick.AddListener(QuitGameButton);
            _playerNickName.onValueChanged.AddListener(ChangeNickName);
        }

        void OnDestroy()
        {
            _hostGameButton.onClick.RemoveAllListeners();
            _joinGameButton.onClick.RemoveAllListeners();
            _quitGameButton.onClick.RemoveAllListeners();
        }

        void HostGameButton()
        {
            HostGameButtonAction.Invoke();
        }

        void JoinGameButton()
        {
            JoinGameButtonAction.Invoke();
        }

        void QuitGameButton()
        {
            QuitGameButtonAction.Invoke();
            Application.Quit(); //TODO extract to another controller
        }

        void ChangeNickName(string name)
        {
            PhotonNetwork.NickName = name;
        }
    }
}
