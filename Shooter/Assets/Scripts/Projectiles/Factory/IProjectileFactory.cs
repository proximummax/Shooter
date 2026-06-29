using Shooter.Abilities;
using UnityEngine;

namespace Shooter.Projectiles
{
    public interface IProjectileFactory
    {
        Projectile Create(
            AbilityDefinition definition,
            GameObject source,
            Vector3 direction,
            int damageLayerMask,
            Vector3 spawnPosition);
    }
}
