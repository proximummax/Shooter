using NUnit.Framework;
using Shooter.Abilities;
using Shooter.Combat;
using Object = UnityEngine.Object;

namespace Shooter.Tests.Abilities
{
    public sealed class AbilityCooldownTests
    {
        [Test]
        public void Cooldown_BlocksUseUntilDurationElapses()
        {
            AreaDamageAbilityEffectDefinition effect = AreaDamageAbilityEffectDefinition.CreateRuntime(
                DamageType.Ability,
                damage: 25f,
                radius: 3f);
            AbilityDefinition definition = AbilityDefinition.CreateRuntime(
                id: "aoe",
                displayName: "AoE Impulse",
                description: "Deals area damage.",
                cooldown: 2f,
                effect);
            var cooldown = new AbilityCooldown(definition);

            try
            {
                Assert.That(cooldown.CanUse(0f), Is.True);

                cooldown.Start(0f);

                Assert.That(cooldown.CanUse(1f), Is.False);
                Assert.That(cooldown.GetNormalizedRemaining(1f), Is.EqualTo(0.5f).Within(0.001f));
                Assert.That(cooldown.CanUse(2.01f), Is.True);
            }
            finally
            {
                Object.DestroyImmediate(definition);
                Object.DestroyImmediate(effect);
            }
        }
    }
}
