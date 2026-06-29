using Shooter.Characters;
using Shooter.Combat;
using UnityEngine;

namespace Shooter.Enemies
{
    public sealed class EnemyAttackExecutor
    {
        private readonly ICombatEffectService _combatEffects;
        private float _nextAttackTime;

        public EnemyAttackExecutor(ICombatEffectService combatEffects = null)
        {
            _combatEffects = combatEffects ?? new CombatEffectService();
        }

        public void Reset()
        {
            _nextAttackTime = 0f;
        }

        public bool TryAttack(Transform target, CharacterStatsDefinition stats, GameObject source, float now)
        {
            if (target == null || stats == null || now < _nextAttackTime)
            {
                return false;
            }

            _nextAttackTime = now + stats.AttackCooldown;
            var intent = new DamageIntent(stats.AttackDamage, DamageType.Enemy, source);
            return _combatEffects.TryApplyDamageToTarget(intent, target, out _);
        }
    }
}
