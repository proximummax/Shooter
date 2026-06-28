using Shooter.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public sealed class AbilitySlotView : MonoBehaviour
    {
        [SerializeField] private Image _cooldownFill;
        [SerializeField] private Text _label;
        [SerializeField] private Text _cooldownText;
        private AbilityCooldown _cooldown;

        public void Initialize(Image cooldownFill, Text label, Text cooldownText)
        {
            _cooldownFill = cooldownFill;
            _label = label;
            _cooldownText = cooldownText;
        }


        public void Bind(AbilityDefinition definition, AbilityCooldown cooldown)
        {
            _cooldown = cooldown;
            SetText(_label, definition != null ? definition.DisplayName : string.Empty);

            gameObject.SetActive(definition != null);
        }

        private void Update()
        {
            if (_cooldown == null)
            {
                return;
            }

            float remaining = _cooldown.GetRemaining(Time.time);
            float normalized = _cooldown.GetNormalizedRemaining(Time.time);
            if (_cooldownFill != null)
            {
                _cooldownFill.fillAmount = normalized;
            }

            SetText(_cooldownText, remaining > 0f ? remaining.ToString("0.0") : string.Empty);
        }

        private static void SetText(Text legacyText, string value)
        {
            if (legacyText != null)
            {
                legacyText.text = value;
            }
        }
    }
}