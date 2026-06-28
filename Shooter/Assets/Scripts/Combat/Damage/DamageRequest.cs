using UnityEngine;

namespace Shooter.Combat
{
    public readonly struct DamageRequest
    {
        public DamageRequest(
            float baseAmount,
            DamageType damageType,
            GameObject source = null,
            IDamageable target = null,
            float armor = 0f,
            float criticalMultiplier = 1f,
            bool forceCritical = false)
        {
            BaseAmount = Mathf.Max(0f, baseAmount);
            DamageType = damageType;
            Source = source;
            Target = target;
            Armor = Mathf.Max(0f, armor);
            CriticalMultiplier = Mathf.Max(1f, criticalMultiplier);
            ForceCritical = forceCritical;
        }

        public float BaseAmount { get; }
        public DamageType DamageType { get; }
        public GameObject Source { get; }
        public IDamageable Target { get; }
        public float Armor { get; }
        public float CriticalMultiplier { get; }
        public bool ForceCritical { get; }

        public DamageRequest WithTarget(IDamageable target)
        {
            return new DamageRequest(BaseAmount, DamageType, Source, target, Armor, CriticalMultiplier, ForceCritical);
        }

        public DamageRequest WithArmor(float armor)
        {
            return new DamageRequest(BaseAmount, DamageType, Source, Target, armor, CriticalMultiplier, ForceCritical);
        }
    }
}
