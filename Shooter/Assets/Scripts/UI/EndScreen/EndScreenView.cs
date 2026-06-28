using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public sealed class EndScreenView : MonoBehaviour, IEndScreenView
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Text _title;
        [SerializeField] private Button _restartButton;

        public event Action RestartClicked;

        public void Initialize(GameObject panel, Text title, Button restartButton)
        {
            _panel = panel;
            _title = title;
            _restartButton = restartButton;
            WireRestartButton();
            SetVisible(false);
        }


        public void SetVisible(bool visible)
        {
            if (_panel != null)
            {
                _panel.SetActive(visible);
            }
        }

        public void SetTitle(string title)
        {
            if (_title != null)
            {
                _title.text = title;
            }
        }

        private void Start()
        {
            WireRestartButton();
        }

        private void OnDestroy()
        {
            if (_restartButton != null)
            {
                _restartButton.onClick.RemoveListener(HandleRestartClicked);
            }
        }

        private void WireRestartButton()
        {
            if (_restartButton == null)
            {
                return;
            }

            _restartButton.onClick.RemoveListener(HandleRestartClicked);
            _restartButton.onClick.AddListener(HandleRestartClicked);
        }

        private void HandleRestartClicked()
        {
            RestartClicked?.Invoke();
        }
    }
}
