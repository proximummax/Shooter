using System;
using System.Collections.Generic;
using Shooter.Abilities;
using Shooter.Effects;
using Shooter.Shared;
using UnityEngine;

namespace Shooter.Projectiles
{
    public sealed class ProjectileFactory : IProjectileFactory
    {
        private readonly Transform _parent;
        private readonly IHitParticleFactory _hitParticleFactory;
        private readonly Dictionary<ProjectileDefinition, ComponentPool<Projectile>> _pools = new Dictionary<ProjectileDefinition, ComponentPool<Projectile>>();
        private readonly Dictionary<Projectile, ComponentPool<Projectile>> _activePools = new Dictionary<Projectile, ComponentPool<Projectile>>();

        public ProjectileFactory(Transform parent, IHitParticleFactory hitParticleFactory)
        {
            _parent = parent;
            _hitParticleFactory = hitParticleFactory;
        }

        public Projectile Create(
            AbilityDefinition definition,
            GameObject source,
            Vector3 direction,
            int damageLayerMask,
            Vector3 spawnPosition)
        {
            ProjectileAbilityEffectDefinition effect = definition != null
                ? definition.GetEffect<ProjectileAbilityEffectDefinition>()
                : null;
            ProjectileDefinition projectileDefinition = effect != null ? effect.Projectile : null;
            if (projectileDefinition == null)
            {
                throw new InvalidOperationException($"{nameof(AbilityDefinition)} '{definition?.Id}' has no {nameof(ProjectileDefinition)}.");
            }

            ComponentPool<Projectile> pool = GetPool(projectileDefinition);
            Projectile projectile = pool.Get();
            _activePools[projectile] = pool;
            projectile.transform.position = spawnPosition;
            projectile.transform.rotation = direction.sqrMagnitude <= 0.001f
                ? Quaternion.identity
                : Quaternion.LookRotation(direction.normalized);
            projectile.Initialize(effect, projectileDefinition, source, direction, damageLayerMask, Release, _hitParticleFactory);
            return projectile;
        }

        private void Release(Projectile projectile)
        {
            if (projectile == null || !_activePools.TryGetValue(projectile, out ComponentPool<Projectile> pool))
            {
                return;
            }

            _activePools.Remove(projectile);
            pool.Release(projectile);
        }

        private ComponentPool<Projectile> GetPool(ProjectileDefinition projectileDefinition)
        {
            if (_pools.TryGetValue(projectileDefinition, out ComponentPool<Projectile> pool))
            {
                return pool;
            }

            if (projectileDefinition.ProjectilePrefab == null)
            {
                throw new InvalidOperationException($"{nameof(ProjectileDefinition)} '{projectileDefinition.Id}' has no projectile prefab.");
            }

            Projectile projectile = projectileDefinition.ProjectilePrefab.GetComponent<Projectile>();
            if (projectile == null)
            {
                throw new InvalidOperationException($"{nameof(ProjectileDefinition)} '{projectileDefinition.Id}' prefab must contain {nameof(Projectile)}.");
            }

            pool = new ComponentPool<Projectile>(projectile, _parent, projectileDefinition.PoolPrewarmCount);
            _pools.Add(projectileDefinition, pool);
            return pool;
        }
    }
}
