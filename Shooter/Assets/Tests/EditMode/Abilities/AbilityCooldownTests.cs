using NUnit.Framework;
using Shooter.Abilities;
using Shooter.Combat;

namespace Shooter.Tests.Abilities
{
    public sealed class AbilityCooldownTests
    {
        [Test]
        public void Cooldown_BlocksUseUntilDurationElapses()
        {
            AbilityDefinition definition = AbilityDefinition.CreateRuntime(
                id: "aoe",
                displayName: "AoE Impulse",
                description: "Deals area damage.",
                type: AbilityType.AreaDamage,
                damageType: DamageType.Ability,
                damage: 25f,
                cooldown: 2f,
                range: 0f,
                radius: 3f,
                projectileSpeed: 0f);
            var cooldown = new AbilityCooldown(definition);

            Assert.That(cooldown.CanUse(0f), Is.True);

            cooldown.Start(0f);

            Assert.That(cooldown.CanUse(1f), Is.False);
            Assert.That(cooldown.GetNormalizedRemaining(1f), Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(cooldown.CanUse(2.01f), Is.True);
        }
    }
}
