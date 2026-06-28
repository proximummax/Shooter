using UnityEngine;

namespace Shooter.Combat
{
    public interface IDamageable
    {
        Transform Transform { get; }
        bool IsDead { get; }
        DamageResult ApplyDamage(DamageRequest request);
    }
}
