using System;
using UnityEngine;

namespace Shooter.Combat
{
    public sealed class CombatEffectService : ICombatEffectService
    {
        private const int MaxOverlapHits = 64;
        private static readonly Collider[] OverlapHits = new Collider[MaxOverlapHits];
        private static readonly IDamageable[] DamagedTargets = new IDamageable[MaxOverlapHits];

        public DamageResult ApplyDamage(DamageIntent intent, IDamageable target)
        {
            if (!CanDamage(intent, target))
            {
                return EmptyResult(intent);
            }

            return target.ApplyDamage(intent.ToRequest(target));
        }

        public bool TryApplyDamageToHit(DamageIntent intent, Collider hitCollider, int layerMask, out DamageResult result)
        {
            result = EmptyResult(intent);
            if (hitCollider == null || !IsInLayerMask(hitCollider.gameObject.layer, layerMask))
            {
                return false;
            }

            IDamageable target = FindDamageable(hitCollider);
            if (!CanDamage(intent, target))
            {
                return false;
            }

            result = ApplyDamage(intent, target);
            return true;
        }

        public bool TryApplyDamageToTarget(DamageIntent intent, Transform target, out DamageResult result)
        {
            result = EmptyResult(intent);
            IDamageable damageable = FindDamageable(target);
            if (!CanDamage(intent, damageable))
            {
                return false;
            }

            result = ApplyDamage(intent, damageable);
            return true;
        }

        public int ApplyAreaDamage(AreaDamageIntent intent)
        {
            if (intent.Radius <= 0f)
            {
                return 0;
            }

            int hitCount = Physics.OverlapSphereNonAlloc(
                intent.Center,
                intent.Radius,
                OverlapHits,
                intent.LayerMask,
                QueryTriggerInteraction.Ignore);
            int damagedCount = 0;

            for (int i = 0; i < hitCount; i++)
            {
                IDamageable target = FindDamageable(OverlapHits[i]);
                if (!CanDamage(intent.Damage, target) || Contains(DamagedTargets, damagedCount, target))
                {
                    continue;
                }

                DamagedTargets[damagedCount] = target;
                damagedCount++;
                ApplyDamage(intent.Damage, target);
            }

            ClearBuffers(hitCount, damagedCount);
            return damagedCount;
        }

        private static IDamageable FindDamageable(Collider hitCollider)
        {
            return hitCollider != null ? hitCollider.GetComponentInParent<IDamageable>() : null;
        }

        private static IDamageable FindDamageable(Transform target)
        {
            return target != null ? target.GetComponentInParent<IDamageable>() : null;
        }

        private static bool CanDamage(DamageIntent intent, IDamageable target)
        {
            return target != null && !target.IsDead && !IsSourceTarget(intent.Source, target);
        }

        private static bool IsSourceTarget(GameObject source, IDamageable target)
        {
            if (source == null || target?.Transform == null)
            {
                return false;
            }

            Transform sourceTransform = source.transform;
            Transform targetTransform = target.Transform;
            return targetTransform == sourceTransform
                || targetTransform.IsChildOf(sourceTransform)
                || sourceTransform.IsChildOf(targetTransform);
        }

        private static bool IsInLayerMask(int layer, int layerMask)
        {
            return (layerMask & (1 << layer)) != 0;
        }

        private static bool Contains(IDamageable[] targets, int count, IDamageable target)
        {
            for (int i = 0; i < count; i++)
            {
                if (Equals(targets[i], target))
                {
                    return true;
                }
            }

            return false;
        }

        private static void ClearBuffers(int hitCount, int damagedCount)
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

        private static DamageResult EmptyResult(DamageIntent intent)
        {
            return new DamageResult(intent.BaseAmount, 0f, false, Array.Empty<string>());
        }
    }
}
