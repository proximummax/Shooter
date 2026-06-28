using Shooter.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.Enemies
{
    public sealed class WorldHealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvasTransform;
        [SerializeField] private Slider _slider;
        private HealthComponent _health;
        private Camera _camera;

        public void Initialize(HealthComponent health, Camera camera)
        {
            if (_health != null)
            {
                _health.HealthChanged -= HandleHealthChanged;
                _health.Died -= HandleDied;
            }

            _health = health;
            _camera = camera;
            CacheView();

            if (_health != null)
            {
                _health.HealthChanged += HandleHealthChanged;
                _health.Died += HandleDied;
            }

            Refresh();
        }

        private void OnDestroy()
        {
            if (_health == null)
            {
                return;
            }

            _health.HealthChanged -= HandleHealthChanged;
            _health.Died -= HandleDied;
        }

        private void LateUpdate()
        {
            if (_camera == null || _canvasTransform == null)
            {
                return;
            }

            Transform cameraTransform = _camera.transform;
            _canvasTransform.LookAt(
                _canvasTransform.position + cameraTransform.rotation * Vector3.forward,
                cameraTransform.rotation * Vector3.up);
        }

        private void HandleHealthChanged(HealthComponent health)
        {
            Refresh();
        }

        private void HandleDied(HealthComponent health)
        {
            if (_canvasTransform != null)
            {
                _canvasTransform.gameObject.SetActive(false);
            }
        }

        private void Refresh()
        {
            if (_slider == null || _health == null)
            {
                return;
            }

            _slider.value = _health.NormalizedHealth;
            if (_canvasTransform != null)
            {
                _canvasTransform.gameObject.SetActive(_camera != null && !_health.IsDead);
            }
        }

        private void CacheView()
        {
            if (_canvasTransform == null)
            {
                _canvasTransform = GetComponentInChildren<RectTransform>(true);
            }

            if (_slider == null)
            {
                _slider = _canvasTransform != null
                    ? _canvasTransform.GetComponentInChildren<Slider>(true)
                    : GetComponentInChildren<Slider>(true);
            }
        }
    }
}
