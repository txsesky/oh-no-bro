using System;
using Game.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMainMenuPanel : UIPanel
    {
        [SerializeField] private TMP_InputField _playerNickName;
        [SerializeField] private Button _hostGameButton;
        [SerializeField] private Button _joinGameButton;
        [SerializeField] private Button _quitGameButton;
        
        [Header("SharedVariable")] 
        [SerializeField] private StringVariable playerNickName;

        public UnityAction HostGameButtonAction;
        public UnityAction JoinGameButtonAction;
        public UnityAction QuitGameButtonAction;

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
        }

        private void ChangeNickName(string name)
        {
            playerNickName.RuntimeValue = name;
        }
    }
}
