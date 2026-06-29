using System;
using System.Collections.Generic;
using System.Linq;
using Shooter.Abilities;
using UnityEngine;

namespace Shooter.Weapons
{
    [CreateAssetMenu(menuName = "Shooter/Weapon")]
    public sealed class WeaponDefinition : ScriptableObject
    {
        [SerializeField] private string _id = "weapon";
        [SerializeField] private string _displayName = "Weapon";
        [SerializeField] private AbilitySlotDefinition[] _abilitySlots = Array.Empty<AbilitySlotDefinition>();

        public string Id => _id;
        public string DisplayName => _displayName;
        public AbilityLoadoutDefinition AbilityLoadout => AbilityLoadoutDefinition.CreateRuntime(_id, _displayName, _abilitySlots);
        public IReadOnlyList<AbilitySlotDefinition> AbilitySlots => AbilityLoadout.AbilitySlots;

        public AbilityDefinition GetAbility(AbilitySlotId slotId)
        {
            return AbilityLoadout.GetAbility(slotId);
        }

        public static WeaponDefinition CreateRuntime(
            string id,
            string displayName,
            IEnumerable<AbilitySlotDefinition> abilitySlots)
        {
            var definition = CreateInstance<WeaponDefinition>();
            definition._id = string.IsNullOrWhiteSpace(id) ? "weapon" : id;
            definition._displayName = string.IsNullOrWhiteSpace(displayName) ? "Weapon" : displayName;
            definition._abilitySlots = NormalizeSlots(abilitySlots);
            return definition;
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
