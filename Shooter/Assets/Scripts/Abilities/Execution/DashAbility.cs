using System;
using UnityEngine;

namespace Shooter.Abilities
{
    public sealed class DashAbility
    {
        private readonly DashAbilityEffectDefinition _effect;

        public DashAbility(AbilityDefinition definition)
        {
            _effect = (definition ?? throw new ArgumentNullException(nameof(definition)))
                .GetEffect<DashAbilityEffectDefinition>();
        }

        public void Execute(AbilityUseContext context)
        {
            Transform caster = context.Caster;
            if (caster == null)
            {
                return;
            }

            Vector3 direction = context.MoveDirection.sqrMagnitude > 0.001f
                ? context.MoveDirection
                : context.AimDirection;
            DashMotor motor = caster.GetComponent<DashMotor>() ?? caster.gameObject.AddComponent<DashMotor>();
            motor.StartDash(direction, _effect.Distance, _effect.Duration);
        }
    }
}
