using System;
using Shooter.Abilities;
using Shooter.Combat;
using UnityEngine;

namespace Shooter.UI
{
    public sealed class HudView : MonoBehaviour, IHudView
    {
        [SerializeField] private HealthBarView _playerHealth;
        [SerializeField] private AbilitySlotView[] _abilitySlots = Array.Empty<AbilitySlotView>();

        public void Initialize(
            HealthBarView playerHealth,
            AbilitySlotView primarySlot,
            AbilitySlotView secondarySlot)
        {
            Initialize(playerHealth, new[] { primarySlot, secondarySlot });
        }

        public void Initialize(
            HealthBarView playerHealth,
            AbilitySlotView[] abilitySlots)
        {
            _playerHealth = playerHealth;
            _abilitySlots = abilitySlots ?? Array.Empty<AbilitySlotView>();
        }

        public void Bind(HealthComponent playerHealth, AbilityLoadoutComponent abilityController)
        {
            _playerHealth.Bind(playerHealth);
            for (int i = 0; i < _abilitySlots.Length; i++)
            {
                AbilitySlotView slotView = _abilitySlots[i];
                if (slotView == null)
                {
                    continue;
                }

                AbilityRuntimeSlot runtimeSlot = i < abilityController.Slots.Count ? abilityController.Slots[i] : null;
                slotView.Bind(runtimeSlot?.Ability, runtimeSlot?.Cooldown);
            }
        }
    }
}
