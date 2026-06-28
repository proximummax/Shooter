using System;
using System.Collections.Generic;
using Shooter.Shared;
using UnityEngine;

namespace Shooter.Enemies
{
    public sealed class EnemyFactory : IEnemyFactory
    {
        private readonly Dictionary<EnemyArchetypeDefinition, ComponentPool<PooledEnemy>> _pools = new Dictionary<EnemyArchetypeDefinition, ComponentPool<PooledEnemy>>();
        private readonly Dictionary<PooledEnemy, ComponentPool<PooledEnemy>> _activePools = new Dictionary<PooledEnemy, ComponentPool<PooledEnemy>>();
        private readonly EnemyRegistry _registry;
        private readonly EnemyRuntimeContext _runtimeContext;
        private readonly Transform _parent;

        public EnemyFactory(
            Transform parent,
            EnemyRegistry registry,
            EnemyRuntimeContext runtimeContext)
        {
            _parent = parent;
            _registry = registry;
            _runtimeContext = runtimeContext ?? throw new ArgumentNullException(nameof(runtimeContext));
        }

        public PooledEnemy Create(EnemyArchetypeDefinition archetype, Vector3 position, string name)
        {
            ComponentPool<PooledEnemy> pool = GetPool(archetype);
            PooledEnemy enemy = pool.Get();
            _activePools[enemy] = pool;
            enemy.name = name;
            enemy.transform.position = position + Vector3.up;
            enemy.transform.rotation = Quaternion.identity;
            enemy.Setup(archetype.Stats, _runtimeContext, Release);
            _registry.Register(enemy.Health);
            enemy.EnableReleaseOnDeath();
            return enemy;
        }

        private ComponentPool<PooledEnemy> GetPool(EnemyArchetypeDefinition archetype)
        {
            if (archetype == null)
            {
                throw new ArgumentNullException(nameof(archetype));
            }

            if (_pools.TryGetValue(archetype, out ComponentPool<PooledEnemy> pool))
            {
                return pool;
            }

            if (archetype.EnemyPrefab == null)
            {
                throw new InvalidOperationException($"{nameof(EnemyArchetypeDefinition)} '{archetype.Id}' has no enemy prefab.");
            }

            if (archetype.Stats == null)
            {
                throw new InvalidOperationException($"{nameof(EnemyArchetypeDefinition)} '{archetype.Id}' has no stats definition.");
            }

            PooledEnemy enemy = archetype.EnemyPrefab.GetComponent<PooledEnemy>();
            if (enemy == null)
            {
                throw new InvalidOperationException($"{nameof(EnemyArchetypeDefinition)} '{archetype.Id}' prefab must contain {nameof(PooledEnemy)}.");
            }

            pool = new ComponentPool<PooledEnemy>(enemy, _parent, archetype.PoolPrewarmCount);
            _pools.Add(archetype, pool);
            return pool;
        }

        private void Release(PooledEnemy enemy)
        {
            if (enemy == null || !_activePools.TryGetValue(enemy, out ComponentPool<PooledEnemy> pool))
            {
                return;
            }

            _activePools.Remove(enemy);
            pool.Release(enemy);
        }
    }
}
