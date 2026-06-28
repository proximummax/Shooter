using System;
using UnityEngine;

namespace Shooter.Abilities
{
    public sealed class DashAbility
    {
        private readonly AbilityDefinition _definition;

        public DashAbility(AbilityDefinition definition)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        public void Execute(AbilityUseContext context)
        {
            DashAbilityEffectDefinition effect = _definition.DashEffect;
            if (effect == null)
            {
                throw new InvalidOperationException($"Ability '{_definition.Id}' has no dash effect configuration.");
            }

            Transform caster = context.Caster;
            if (caster == null)
            {
                return;
            }

            Vector3 direction = context.MoveDirection.sqrMagnitude > 0.001f
                ? context.MoveDirection
                : context.AimDirection;
            DashMotor motor = caster.GetComponent<DashMotor>() ?? caster.gameObject.AddComponent<DashMotor>();
            motor.StartDash(direction, effect.Distance, effect.Duration);
        }
    }
}
