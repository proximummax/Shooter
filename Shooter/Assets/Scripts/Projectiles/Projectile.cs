using System;
using Shooter.Abilities;
using Shooter.Combat;
using Shooter.Effects;
using UnityEngine;

namespace Shooter.Projectiles
{
    [RequireComponent(typeof(Collider))]
    public sealed class Projectile : MonoBehaviour
    {
        private ProjectileAbilityEffectDefinition _effect;
        private Vector3 _direction;
        private float _travelledDistance;
        private bool _isInitialized;
        private bool _isReleased;
        private Action<Projectile> _release;
        private ProjectileImpactResolver _impactResolver;

        public void Initialize(
            ProjectileAbilityEffectDefinition effect,
            ProjectileDefinition projectileDefinition,
            GameObject source,
            Vector3 direction,
            int damageLayerMask,
            Action<Projectile> release,
            IHitParticleFactory hitParticleFactory,
            ICombatEffectService combatEffects)
        {
            _effect = effect;
            _direction = direction.sqrMagnitude <= 0.001f ? Vector3.forward : direction.normalized;
            _travelledDistance = 0f;
            _release = release;
            _impactResolver = new ProjectileImpactResolver(effect, projectileDefinition, source, damageLayerMask, hitParticleFactory, combatEffects);
            _isInitialized = true;
            _isReleased = false;

            Collider projectileCollider = GetComponent<Collider>();
            projectileCollider.isTrigger = true;

            Rigidbody body = GetComponent<Rigidbody>();
            if (body == null)
            {
                body = gameObject.AddComponent<Rigidbody>();
            }

            body.useGravity = false;
            body.isKinematic = true;
        }

        private void Update()
        {
            if (!_isInitialized)
            {
                return;
            }

            float step = _effect.ProjectileSpeed * Time.deltaTime;
            transform.position += _direction * step;
            _travelledDistance += step;

            if (_travelledDistance >= _effect.Range)
            {
                Release();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isInitialized || _impactResolver == null)
            {
                return;
            }

            var context = new ProjectileImpactContext(other, transform.position, -_direction);
            if (_impactResolver.TryResolve(context))
            {
                Release();
            }
        }

        public void Release()
        {
            if (_isReleased)
            {
                return;
            }

            _isReleased = true;
            _isInitialized = false;
            _effect = null;
            _impactResolver = null;

            if (_release != null)
            {
                _release(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
