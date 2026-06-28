using System;
using NUnit.Framework;
using Shooter.Abilities;
using Shooter.Arena;
using Shooter.Combat;
using Shooter.Composition;
using Shooter.Characters;
using Shooter.Enemies;
using Shooter.Weapons;

namespace Shooter.Tests.Composition
{
    public sealed class RuntimeValidationTests
    {
        [Test]
        public void RuntimeContentValidator_RejectsProjectileAbilityWithoutProjectileDefinition()
        {
            AbilityDefinition ability = AbilityDefinition.CreateRuntime(
                "shot",
                "Shot",
                "Projectile attack.",
                AbilityType.Projectile,
                DamageType.Basic,
                10f,
                0.2f,
                10f,
                0f,
                8f);
            WeaponDefinition weapon = WeaponDefinition.CreateRuntime(
                "starter",
                "Starter",
                new[] { AbilitySlotDefinition.CreateRuntime(AbilitySlotId.Primary, ability) });

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                RuntimeContentValidator.RequireValidWeapon(weapon, "Player Feature Scope"));

            Assert.That(exception.Message, Does.Contain("shot"));
            Assert.That(exception.Message, Does.Contain("projectile definition"));
        }

        [Test]
        public void RuntimeContentValidator_RejectsWaveEnemyWithoutPrefab()
        {
            CharacterStatsDefinition stats = CharacterStatsDefinition.CreateRuntime(
                "Enemy",
                10f,
                0f,
                1f,
                1f,
                1f,
                1f);
            EnemyArchetypeDefinition archetype = EnemyArchetypeDefinition.CreateRuntime(
                "enemy",
                null,
                stats);
            WaveDefinition wave = WaveDefinition.CreateRuntime(archetype, 1, 3f);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                RuntimeContentValidator.RequireValidWave(wave, "Enemy Feature Scope"));

            Assert.That(exception.Message, Does.Contain("enemy"));
            Assert.That(exception.Message, Does.Contain("enemy prefab"));
        }
    }
}
