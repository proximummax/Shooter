using System;
using System.Collections.Generic;
using System.Linq;

namespace Shooter.Abilities
{
    public sealed class AbilityLoadoutDefinition
    {
        private readonly AbilitySlotDefinition[] _abilitySlots;

        private AbilityLoadoutDefinition(
            string id,
            string displayName,
            IEnumerable<AbilitySlotDefinition> abilitySlots)
        {
            Id = string.IsNullOrWhiteSpace(id) ? "loadout" : id;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? "Loadout" : displayName;
            _abilitySlots = NormalizeSlots(abilitySlots);
        }

        public string Id { get; }
        public string DisplayName { get; }
        public IReadOnlyList<AbilitySlotDefinition> AbilitySlots => _abilitySlots;

        public AbilityDefinition GetAbility(AbilitySlotId slotId)
        {
            return _abilitySlots
                .FirstOrDefault(slot => slot != null && slot.SlotId == slotId)
                ?.Ability;
        }

        public static AbilityLoadoutDefinition CreateRuntime(
            string id,
            string displayName,
            IEnumerable<AbilitySlotDefinition> abilitySlots)
        {
            return new AbilityLoadoutDefinition(id, displayName, abilitySlots);
        }

        private static AbilitySlotDefinition[] NormalizeSlots(IEnumerable<AbilitySlotDefinition> abilitySlots)
        {
            return abilitySlots?
                .Where(slot => slot != null && slot.HasAbility)
                .GroupBy(slot => slot.SlotId)
                .Select(group => group.First())
                .ToArray() ?? Array.Empty<AbilitySlotDefinition>();
        }
    }
}
