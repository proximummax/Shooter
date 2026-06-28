using Shooter.Combat;
using UnityEngine;

namespace Shooter.Abilities
{
    public sealed class AreaDamageAbility
    {
        private const int MaxOverlapHits = 64;
        private static readonly Collider[] OverlapHits = new Collider[MaxOverlapHits];
        private static readonly HealthComponent[] DamagedTargets = new HealthComponent[MaxOverlapHits];
        private readonly AbilityDefinition _definition;
        private readonly DamagePipeline _pipeline;
        private readonly int _damageLayerMask;

        public AreaDamageAbility(AbilityDefinition definition, DamagePipeline pipeline, int damageLayerMask)
        {
            _definition = definition;
            _pipeline = pipeline ?? DamagePipeline.Default;
            _damageLayerMask = damageLayerMask;
        }

        public int Execute(AbilityUseContext context)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(
                context.Caster.position,
                _definition.Radius,
                OverlapHits,
                _damageLayerMask,
                QueryTriggerInteraction.Ignore);
            int damagedCount = 0;

            for (int i = 0; i < hitCount; i++)
            {
                HealthComponent health = OverlapHits[i].GetComponentInParent<HealthComponent>();
                if (health == null || health.IsDead || health.transform == context.Caster || Contains(DamagedTargets, damagedCount, health))
                {
                    continue;
                }

                DamagedTargets[damagedCount] = health;
                damagedCount++;
                health.ApplyDamage(new DamageRequest(
                    baseAmount: _definition.Damage,
                    damageType: _definition.DamageType,
                    source: context.Caster.gameObject,
                    target: health));
            }

            ClearBuffers(hitCount, damagedCount);
            return damagedCount;
        }

        private static bool Contains(HealthComponent[] targets, int count, HealthComponent target)
        {
            for (int i = 0; i < count; i++)
            {
                if (targets[i] == target)
                {
                    return true;
                }
            }

            return false;
        }

        private void ClearBuffers(int hitCount, int damagedCount)
        {
            for (int i = 0; i < hitCount; i++)
            {
                OverlapHits[i] = null;
            }

            for (int i = 0; i < damagedCount; i++)
            {
                DamagedTargets[i] = null;
            }
        }
    }
}
