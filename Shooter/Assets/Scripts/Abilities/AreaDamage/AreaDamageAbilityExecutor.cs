using Shooter.Combat;

namespace Shooter.Abilities
{
    public sealed class AreaDamageAbilityExecutor : IAbilityExecutor
    {
        private readonly ICombatEffectService _combatEffects;

        public AreaDamageAbilityExecutor(ICombatEffectService combatEffects)
        {
            _combatEffects = combatEffects;
        }

        public AbilityType AbilityType => AbilityType.AreaDamage;

        public void Execute(AbilityDefinition ability, AbilityUseContext context, int damageLayerMask)
        {
            new AreaDamageAbility(ability, _combatEffects, damageLayerMask).Execute(context);
        }
    }
}
