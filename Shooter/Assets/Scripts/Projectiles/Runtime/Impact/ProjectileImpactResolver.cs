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
        private readonly ICombatEffectService _combatEffects;

        public ProjectileImpactResolver(
            ProjectileAbilityEffectDefinition effect,
            ProjectileDefinition projectile,
            GameObject source,
            int damageLayerMask,
            IHitParticleFactory hitParticleFactory,
            ICombatEffectService combatEffects)
        {
            _effect = effect;
            _projectile = projectile;
            _source = source;
            _damageLayerMask = damageLayerMask;
            _hitParticleFactory = hitParticleFactory;
            _combatEffects = combatEffects ?? new CombatEffectService();
        }

        public bool TryResolve(ProjectileImpactContext context)
        {
            Collider other = context.Collider;
            if (_effect == null || other == null)
            {
                return false;
            }

            var intent = new DamageIntent(_effect.Damage, _effect.DamageType, _source);
            if (!_combatEffects.TryApplyDamageToHit(intent, other, _damageLayerMask, out _))
            {
                return false;
            }

            _hitParticleFactory?.Play(_projectile != null ? _projectile.HitParticlePrefab : null, context.Position, context.Normal);
            return true;
        }
    }
}
