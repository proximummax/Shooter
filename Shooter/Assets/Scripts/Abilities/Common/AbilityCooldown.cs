using UnityEngine;

namespace Shooter.Abilities
{
    public sealed class AbilityCooldown
    {
        private readonly AbilityDefinition _definition;
        private float _readyAt;

        public AbilityCooldown(AbilityDefinition definition)
        {
            _definition = definition;
            _readyAt = 0f;
        }

        public AbilityDefinition Definition => _definition;

        public bool CanUse(float now)
        {
            return now >= _readyAt;
        }

        public void Start(float now)
        {
            _readyAt = now + _definition.Cooldown;
        }

        public float GetRemaining(float now)
        {
            return Mathf.Max(0f, _readyAt - now);
        }

        public float GetNormalizedRemaining(float now)
        {
            if (_definition.Cooldown <= 0f)
            {
                return 0f;
            }

            return Mathf.Clamp01(GetRemaining(now) / _definition.Cooldown);
        }
    }
}
