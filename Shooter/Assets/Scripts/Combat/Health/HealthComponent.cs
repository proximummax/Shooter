using System;
using Shooter.Characters;
using UnityEngine;

namespace Shooter.Combat
{
    public sealed class HealthComponent : MonoBehaviour, IDamageable
    {
        private float _maxHealth = 100f;
        private float _currentHealth = 100f;
        private float _armor;
        private DamagePipeline _pipeline = DamagePipeline.Default;
        private bool _isInitialized;

        public event Action<HealthComponent> HealthChanged;
        public event Action<HealthComponent> Died;

        public Transform Transform => transform;
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public float NormalizedHealth => _maxHealth <= 0f ? 0f : _currentHealth / _maxHealth;
        public bool IsDead { get; private set; }

        private void Awake()
        {
            if (!_isInitialized)
            {
                Initialize(_maxHealth, _armor, _pipeline);
            }
        }

        public void Initialize(CharacterStatsDefinition stats, DamagePipeline pipeline)
        {
            Initialize(stats.MaxHealth, stats.Armor, pipeline);
        }

        public void Initialize(float maxHealth, float armor, DamagePipeline pipeline)
        {
            _maxHealth = Mathf.Max(1f, maxHealth);
            _currentHealth = _maxHealth;
            _armor = Mathf.Max(0f, armor);
            _pipeline = pipeline ?? DamagePipeline.Default;
            IsDead = false;
            _isInitialized = true;
            HealthChanged?.Invoke(this);
        }

        public DamageResult ApplyDamage(DamageRequest request)
        {
            if (IsDead)
            {
                return new DamageResult(request.BaseAmount, 0f, false, Array.Empty<string>());
            }

            DamageRequest contextualRequest = request
                .WithTarget(this)
                .WithArmor(_armor);
            DamageResult result = _pipeline.Apply(contextualRequest);
            if (result.FinalAmount <= 0f)
            {
                return result;
            }

            float previousHealth = _currentHealth;
            _currentHealth = Mathf.Clamp(_currentHealth - result.FinalAmount, 0f, _maxHealth);

            if (!Mathf.Approximately(previousHealth, _currentHealth))
            {
                HealthChanged?.Invoke(this);
            }

            if (_currentHealth <= 0f && !IsDead)
            {
                IsDead = true;
                Died?.Invoke(this);
            }

            return result;
        }

    }
}
