using NUnit.Framework;
using Shooter.Characters;
using Shooter.Combat;
using Shooter.Enemies;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shooter.Tests.Enemies
{
    public sealed class EnemyAttackExecutorTests
    {
        [Test]
        public void TryAttack_AppliesDamageThroughTargetHealthAndRespectsCooldown()
        {
            GameObject source = new GameObject("enemy");
            GameObject target = new GameObject("player");
            HealthComponent health = target.AddComponent<HealthComponent>();
            health.Initialize(20f, 0f, DamagePipeline.Default);
            CharacterStatsDefinition stats = CharacterStatsDefinition.CreateRuntime(
                "Enemy",
                10f,
                0f,
                1f,
                1f,
                4f,
                1f);
            var executor = new EnemyAttackExecutor();

            try
            {
                bool firstAttack = executor.TryAttack(target.transform, stats, source, 0f);
                bool cooldownAttack = executor.TryAttack(target.transform, stats, source, 0.5f);
                bool secondAttack = executor.TryAttack(target.transform, stats, source, 1f);

                Assert.That(firstAttack, Is.True);
                Assert.That(cooldownAttack, Is.False);
                Assert.That(secondAttack, Is.True);
                Assert.That(health.CurrentHealth, Is.EqualTo(12f));
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(target);
                Object.DestroyImmediate(stats);
            }
        }
    }
}
