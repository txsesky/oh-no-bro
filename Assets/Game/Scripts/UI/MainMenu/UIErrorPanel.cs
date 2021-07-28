using System;
using Game.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIErrorPanel : UIPanel
    {
        [SerializeField] private TMP_Text _errorMessageText;
        [SerializeField] private Button _backButton;
        
        public UnityAction BackButtonAction;

        private void Awake()
        {
            _backButton.onClick.AddListener(BackButton);
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveAllListeners();
        }

        public void SetErrorMessage(string message)
        {
            _errorMessageText.text = message;
        }

        private void BackButton()
        {
            BackButtonAction.Invoke();
        }
    }
}