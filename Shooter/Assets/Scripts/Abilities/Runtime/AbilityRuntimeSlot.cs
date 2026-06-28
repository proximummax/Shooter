namespace Shooter.Abilities
{
    public sealed class AbilityRuntimeSlot
    {
        public AbilityRuntimeSlot(AbilitySlotDefinition definition)
        {
            Definition = definition;
            SlotId = definition.SlotId;
            Ability = definition.Ability;
            Cooldown = new AbilityCooldown(Ability);
        }

        public AbilitySlotDefinition Definition { get; }
        public AbilitySlotId SlotId { get; }
        public AbilityDefinition Ability { get; }
        public AbilityCooldown Cooldown { get; }
    }
}
