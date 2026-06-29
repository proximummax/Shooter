using System;
using Shooter.Characters;
using Shooter.Combat;
using UnityEngine;

namespace Shooter.Enemies
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(EnemyActor))]
    public sealed class PooledEnemy : MonoBehaviour
    {
        private HealthComponent _health;
        private EnemyActor _actor;
        private Collider[] _colliders;
        private Renderer[] _renderers;
        private Action<PooledEnemy> _release;
        private bool _releaseSubscribed;

        public HealthComponent Health => _health != null ? _health : _health = GetComponent<HealthComponent>();

        private void Awake()
        {
            CacheComponents();
        }

        public void Setup(
            CharacterStatsDefinition stats,
            EnemyRuntimeContext runtimeContext,
            Action<PooledEnemy> release)
        {
            CacheComponents();
            if (runtimeContext == null)
            {
                throw new ArgumentNullException(nameof(runtimeContext));
            }

            _release = release;
            RestorePresentation();

            Health.Initialize(stats, runtimeContext.DamagePipeline);
            _actor.Initialize(runtimeContext.Target, stats, runtimeContext.SessionState, runtimeContext.CombatEffects);

            WorldHealthBar healthBar = GetComponentInChildren<WorldHealthBar>(true);
            if (healthBar != null)
            {
                healthBar.Initialize(Health, runtimeContext.WorldUiCamera);
            }
        }

        public void EnableReleaseOnDeath()
        {
            if (_releaseSubscribed)
            {
                return;
            }

            Health.Died += HandleDied;
            _releaseSubscribed = true;
        }

        public void Release()
        {
            if (_releaseSubscribed)
            {
                Health.Died -= HandleDied;
                _releaseSubscribed = false;
            }

            _release?.Invoke(this);
        }

        private void HandleDied(HealthComponent health)
        {
            Release();
        }

        private void CacheComponents()
        {
            if (_health == null)
            {
                _health = GetComponent<HealthComponent>();
            }

            if (_actor == null)
            {
                _actor = GetComponent<EnemyActor>();
            }

            _colliders = GetComponentsInChildren<Collider>(true);
            _renderers = GetComponentsInChildren<Renderer>(true);
        }

        private void RestorePresentation()
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = true;
            }

            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].enabled = true;
            }

            _actor.enabled = true;
        }
    }
}
