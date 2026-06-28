using NUnit.Framework;
using Shooter.Abilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shooter.Tests.Abilities
{
    public sealed class DashAbilityTests
    {
        [Test]
        public void Execute_MovesCasterOverDashDurationUsingMovementDirection()
        {
            DashAbilityEffectDefinition effect = DashAbilityEffectDefinition.CreateRuntime(
                distance: 4f,
                duration: 0.2f);
            AbilityDefinition definition = AbilityDefinition.CreateRuntime(
                "dash",
                "Dash",
                "Short movement burst.",
                cooldown: 1f,
                effect);
            var caster = new GameObject("caster");

            try
            {
                var ability = new DashAbility(definition);

                ability.Execute(new AbilityUseContext(caster.transform, Vector3.forward, Vector3.right, 0f));

                DashMotor motor = caster.GetComponent<DashMotor>();
                Assert.That(motor, Is.Not.Null);
                Assert.That(motor.IsDashing, Is.True);

                motor.Tick(0.1f);
                Assert.That(Vector3.Distance(caster.transform.position, Vector3.right * 2f), Is.LessThan(0.001f));

                motor.Tick(0.1f);
                Assert.That(Vector3.Distance(caster.transform.position, Vector3.right * 4f), Is.LessThan(0.001f));
                Assert.That(motor.IsDashing, Is.False);
            }
            finally
            {
                Object.DestroyImmediate(caster);
                Object.DestroyImmediate(definition);
                Object.DestroyImmediate(effect);
            }
        }
    }
}
