using UnityEngine;

namespace Shooter.Combat
{
    public interface ICombatEffectService
    {
        DamageResult ApplyDamage(DamageIntent intent, IDamageable target);
        bool TryApplyDamageToHit(DamageIntent intent, Collider hitCollider, int layerMask, out DamageResult result);
        bool TryApplyDamageToTarget(DamageIntent intent, Transform target, out DamageResult result);
        int ApplyAreaDamage(AreaDamageIntent intent);
    }
}
