using System;
using UnityEngine;

namespace Shooter.Abilities
{
    [Serializable]
    public sealed class AbilitySlotDefinition
    {
        [SerializeField] private AbilitySlotId _slotId = AbilitySlotId.Primary;
        [SerializeField] private AbilityDefinition _ability;

        public AbilitySlotId SlotId => _slotId;
        public AbilityDefinition Ability => _ability;
        public bool HasAbility => _ability != null;

        public static AbilitySlotDefinition CreateRuntime(AbilitySlotId slotId, AbilityDefinition ability)
        {
            return new AbilitySlotDefinition
            {
                _slotId = slotId,
                _ability = ability
            };
        }
    }
}
