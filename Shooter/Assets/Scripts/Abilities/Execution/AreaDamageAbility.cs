using Shooter.Combat;
using UnityEngine;

namespace Shooter.Abilities
{
    public sealed class AreaDamageAbility
    {
        private readonly AreaDamageAbilityEffectDefinition _effect;
        private readonly ICombatEffectService _combatEffects;
        private readonly int _damageLayerMask;

        public AreaDamageAbility(AbilityDefinition definition, ICombatEffectService combatEffects, int damageLayerMask)
        {
            _effect = definition.GetEffect<AreaDamageAbilityEffectDefinition>();
            _combatEffects = combatEffects ?? new CombatEffectService();
            _damageLayerMask = damageLayerMask;
        }

        public int Execute(AbilityUseContext context)
        {
            var damage = new DamageIntent(_effect.Damage, _effect.DamageType, context.Caster.gameObject);
            var areaDamage = new AreaDamageIntent(damage, context.Caster.position, _effect.Radius, _damageLayerMask);
            return _combatEffects.ApplyAreaDamage(areaDamage);
        }
    }
}
