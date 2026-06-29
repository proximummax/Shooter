using Shooter.Abilities;
using Shooter.Combat;
using Shooter.Effects;
using UnityEngine;

namespace Shooter.Projectiles
{
    public sealed class ProjectileImpactResolver
    {
        private readonly ProjectileAbilityEffectDefinition _effect;
        private readonly ProjectileDefinition _projectile;
        private readonly GameObject _source;
        private readonly int _damageLayerMask;
        private readonly IHitParticleFactory _hitParticleFactory;

        public ProjectileImpactResolver(
            ProjectileAbilityEffectDefinition effect,
            ProjectileDefinition projectile,
            GameObject source,
            int damageLayerMask,
            IHitParticleFactory hitParticleFactory)
        {
            _effect = effect;
            _projectile = projectile;
            _source = source;
            _damageLayerMask = damageLayerMask;
            _hitParticleFactory = hitParticleFactory;
        }

        public bool TryResolve(ProjectileImpactContext context)
        {
            Collider other = context.Collider;
            if (_effect == null || other == null || other.gameObject == _source || ((_damageLayerMask & (1 << other.gameObject.layer)) == 0))
            {
                return false;
            }

            HealthComponent health = other.GetComponentInParent<HealthComponent>();
            if (health == null || health.IsDead || health.gameObject == _source)
            {
                return false;
            }

            health.ApplyDamage(new DamageRequest(
                baseAmount: _effect.Damage,
                damageType: _effect.DamageType,
                source: _source,
                target: health));

            _hitParticleFactory?.Play(_projectile != null ? _projectile.HitParticlePrefab : null, context.Position, context.Normal);
            return true;
        }
    }
}
