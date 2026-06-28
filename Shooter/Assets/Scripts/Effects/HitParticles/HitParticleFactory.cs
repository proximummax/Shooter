using System;
using System.Collections.Generic;
using Shooter.Shared;
using UnityEngine;

namespace Shooter.Effects
{
    public sealed class HitParticleFactory : IHitParticleFactory
    {
        private readonly Transform _parent;
        private readonly Dictionary<GameObject, ComponentPool<HitParticleEffect>> _pools = new Dictionary<GameObject, ComponentPool<HitParticleEffect>>();
        private readonly Dictionary<HitParticleEffect, ComponentPool<HitParticleEffect>> _activePools = new Dictionary<HitParticleEffect, ComponentPool<HitParticleEffect>>();

        public HitParticleFactory(Transform parent)
        {
            _parent = parent;
        }

        public HitParticleEffect Play(GameObject prefab, Vector3 position, Vector3 normal)
        {
            if (prefab == null)
            {
                return null;
            }

            ComponentPool<HitParticleEffect> pool = GetPool(prefab);
            HitParticleEffect effect = pool.Get();
            _activePools[effect] = pool;
            effect.Initialize(Release);
            effect.Play(position, normal);
            return effect;
        }

        private void Release(HitParticleEffect effect)
        {
            if (effect == null || !_activePools.TryGetValue(effect, out ComponentPool<HitParticleEffect> pool))
            {
                return;
            }

            _activePools.Remove(effect);
            pool.Release(effect);
        }

        private ComponentPool<HitParticleEffect> GetPool(GameObject prefab)
        {
            if (_pools.TryGetValue(prefab, out ComponentPool<HitParticleEffect> pool))
            {
                return pool;
            }

            HitParticleEffect effect = prefab.GetComponent<HitParticleEffect>();
            if (effect == null)
            {
                throw new InvalidOperationException($"Hit particle prefab '{prefab.name}' must contain {nameof(HitParticleEffect)}.");
            }

            pool = new ComponentPool<HitParticleEffect>(effect, _parent);
            _pools.Add(prefab, pool);
            return pool;
        }
    }
}
