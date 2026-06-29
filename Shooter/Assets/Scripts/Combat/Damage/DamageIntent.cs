using UnityEngine;

namespace Shooter.Combat
{
    public readonly struct DamageIntent
    {
        public DamageIntent(
            float baseAmount,
            DamageType damageType,
            GameObject source = null,
            float criticalMultiplier = 1f,
            bool forceCritical = false)
        {
            BaseAmount = Mathf.Max(0f, baseAmount);
            DamageType = damageType;
            Source = source;
            CriticalMultiplier = Mathf.Max(1f, criticalMultiplier);
            ForceCritical = forceCritical;
        }

        public float BaseAmount { get; }
        public DamageType DamageType { get; }
        public GameObject Source { get; }
        public float CriticalMultiplier { get; }
        public bool ForceCritical { get; }

        public DamageRequest ToRequest(IDamageable target)
        {
            return new DamageRequest(
                BaseAmount,
                DamageType,
                Source,
                target,
                criticalMultiplier: CriticalMultiplier,
                forceCritical: ForceCritical);
        }
    }
}
