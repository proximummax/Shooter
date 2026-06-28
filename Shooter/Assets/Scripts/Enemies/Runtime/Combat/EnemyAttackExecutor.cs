using Shooter.Characters;
using Shooter.Combat;
using UnityEngine;

namespace Shooter.Enemies
{
    public sealed class EnemyAttackExecutor
    {
        private float _nextAttackTime;

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
            HealthComponent targetHealth = target.GetComponent<HealthComponent>();
            if (targetHealth == null || targetHealth.IsDead)
            {
                return false;
            }

            targetHealth.ApplyDamage(new DamageRequest(
                baseAmount: stats.AttackDamage,
                damageType: DamageType.Enemy,
                source: source,
                target: targetHealth));
            return true;
        }
    }
}
