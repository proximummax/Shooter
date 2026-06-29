using System;
using NUnit.Framework;
using Shooter.Abilities;
using Shooter.Combat;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shooter.Tests.Abilities
{
    public sealed class AbilityDefinitionTests
    {
        [Test]
        public void GetEffect_ReturnsConfiguredEffectWhenTypeMatches()
        {
            AreaDamageAbilityEffectDefinition effect = AreaDamageAbilityEffectDefinition.CreateRuntime(
                DamageType.Ability,
                damage: 12f,
                radius: 3f);
            AbilityDefinition definition = AbilityDefinition.CreateRuntime(
                "aoe",
                "AoE",
                "Area damage.",
                cooldown: 1f,
                effect);

            try
            {
                AreaDamageAbilityEffectDefinition actual = definition.GetEffect<AreaDamageAbilityEffectDefinition>();

                Assert.That(actual, Is.SameAs(effect));
            }
            finally
            {
                Object.DestroyImmediate(definition);
                Object.DestroyImmediate(effect);
            }
        }

        [Test]
        public void GetEffect_ThrowsUsefulErrorWhenTypeDoesNotMatch()
        {
            DashAbilityEffectDefinition effect = DashAbilityEffectDefinition.CreateRuntime(
                distance: 4f,
                duration: 0.15f);
            AbilityDefinition definition = AbilityDefinition.CreateRuntime(
                "dash",
                "Dash",
                "Movement burst.",
                cooldown: 1f,
                effect);

            try
            {
                InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
                    () => definition.GetEffect<AreaDamageAbilityEffectDefinition>());

                Assert.That(exception.Message, Does.Contain("dash"));
                Assert.That(exception.Message, Does.Contain(nameof(AreaDamageAbilityEffectDefinition)));
            }
            finally
            {
                Object.DestroyImmediate(definition);
                Object.DestroyImmediate(effect);
            }
        }
    }
}
