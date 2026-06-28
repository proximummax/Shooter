using Shooter.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public sealed class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _label;
        private HealthComponent _health;

        public void Initialize(Slider slider, Text label)
        {
            _slider = slider;
            _label = label;
        }

        public void Bind(HealthComponent health)
        {
            if (_health != null)
            {
                _health.HealthChanged -= Refresh;
            }

            _health = health;
            if (_health != null)
            {
                _health.HealthChanged += Refresh;
                Refresh(_health);
            }
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.HealthChanged -= Refresh;
            }
        }

        private void Refresh(HealthComponent health)
        {
            if (_slider != null)
            {
                _slider.value = health.NormalizedHealth;
            }

            string value = $"{Mathf.CeilToInt(health.CurrentHealth)} / {Mathf.CeilToInt(health.MaxHealth)}";
            if (_label != null)
            {
                _label.text = value;
            }
        }
    }
}