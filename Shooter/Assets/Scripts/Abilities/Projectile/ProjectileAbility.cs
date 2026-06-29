using Shooter.Projectiles;
using UnityEngine;

namespace Shooter.Abilities
{
    public sealed class ProjectileAbility
    {
        private readonly AbilityDefinition _definition;
        private readonly IProjectileFactory _projectileFactory;
        private readonly int _damageLayerMask;

        public ProjectileAbility(AbilityDefinition definition, IProjectileFactory projectileFactory, int damageLayerMask)
        {
            _definition = definition;
            _projectileFactory = projectileFactory;
            _damageLayerMask = damageLayerMask;
        }

        public Projectile Execute(AbilityUseContext context)
        {
            Vector3 spawnPosition = context.Caster.position + Vector3.up * 0.8f + context.AimDirection * 0.8f;
            return _projectileFactory?.Create(_definition, context.Caster.gameObject, context.AimDirection, _damageLayerMask, spawnPosition);
        }
    }
}
