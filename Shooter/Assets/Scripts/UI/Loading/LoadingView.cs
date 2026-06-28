using System;
using System.Collections;
using System.Collections.Generic;
using Shooter.Loading;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public sealed class LoadingView : MonoBehaviour, ILoadingView
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Slider _progress;
        [SerializeField] private Text _status;
        [SerializeField] private Button _retryButton;

        public event Action RetryClicked;

        public void Initialize(GameObject panel, Slider progress, Text status, Button retryButton)
        {
            _panel = panel;
            _progress = progress;
            _status = status;
            _retryButton = retryButton;
            WireRetryButton();
        }

        public void SetVisible(bool visible)
        {
            if (_panel != null)
            {
                _panel.SetActive(visible);
            }
        }

        public void SetProgress(float progress)
        {
            if (_progress != null)
            {
                _progress.value = progress;
            }
        }

        public void SetStatusText(string statusText)
        {
            if (_status != null)
            {
                _status.text = statusText;
            }
        }

        public void SetRetryVisible(bool visible)
        {
            if (_retryButton != null)
            {
                _retryButton.gameObject.SetActive(visible);
            }
        }

        public void PlaySnapshots(
            IEnumerable<LoadingSnapshot> snapshots,
            Func<LoadingSnapshot, float> delayProvider,
            Action<LoadingSnapshot> onSnapshot)
        {
            StopAllCoroutines();
            StartCoroutine(RunSnapshots(snapshots, delayProvider, onSnapshot));
        }

        private void Start()
        {
            WireRetryButton();
        }

        private void OnDestroy()
        {
            if (_retryButton != null)
            {
                _retryButton.onClick.RemoveListener(HandleRetryClicked);
            }
        }

        private void WireRetryButton()
        {
            if (_retryButton == null)
            {
                return;
            }

            _retryButton.onClick.RemoveListener(HandleRetryClicked);
            _retryButton.onClick.AddListener(HandleRetryClicked);
        }

        private void HandleRetryClicked()
        {
            RetryClicked?.Invoke();
        }

        private IEnumerator RunSnapshots(
            IEnumerable<LoadingSnapshot> snapshots,
            Func<LoadingSnapshot, float> delayProvider,
            Action<LoadingSnapshot> onSnapshot)
        {
            foreach (LoadingSnapshot snapshot in snapshots)
            {
                onSnapshot?.Invoke(snapshot);
                float delay = delayProvider != null ? delayProvider(snapshot) : 0f;
                if (delay > 0f)
                {
                    yield return new WaitForSecondsRealtime(delay);
                }
            }
        }
    }
}