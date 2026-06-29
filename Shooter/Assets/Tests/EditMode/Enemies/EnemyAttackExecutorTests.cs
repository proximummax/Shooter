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
        public void TryAttack_DelegatesDamageToCombatEffectServiceAndRespectsCooldown()
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
            var combatEffects = new RecordingCombatEffectService();
            var executor = new EnemyAttackExecutor(combatEffects);

            try
            {
                bool firstAttack = executor.TryAttack(target.transform, stats, source, 0f);
                bool cooldownAttack = executor.TryAttack(target.transform, stats, source, 0.5f);
                bool secondAttack = executor.TryAttack(target.transform, stats, source, 1f);

                Assert.That(firstAttack, Is.True);
                Assert.That(cooldownAttack, Is.False);
                Assert.That(secondAttack, Is.True);
                Assert.That(combatEffects.CallCount, Is.EqualTo(2));
                Assert.That(combatEffects.LastIntent.BaseAmount, Is.EqualTo(4f));
                Assert.That(combatEffects.LastIntent.DamageType, Is.EqualTo(DamageType.Enemy));
                Assert.That(combatEffects.LastIntent.Source, Is.EqualTo(source));
                Assert.That(combatEffects.LastTarget, Is.EqualTo(target.transform));
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(target);
                Object.DestroyImmediate(stats);
            }
        }

        private sealed class RecordingCombatEffectService : ICombatEffectService
        {
            public int CallCount { get; private set; }
            public DamageIntent LastIntent { get; private set; }
            public Transform LastTarget { get; private set; }

            public DamageResult ApplyDamage(DamageIntent intent, IDamageable target)
            {
                CallCount++;
                LastIntent = intent;
                return new DamageResult(intent.BaseAmount, intent.BaseAmount, false, System.Array.Empty<string>());
            }

            public bool TryApplyDamageToHit(DamageIntent intent, Collider hitCollider, int layerMask, out DamageResult result)
            {
                result = ApplyDamage(intent, null);
                return true;
            }

            public bool TryApplyDamageToTarget(DamageIntent intent, Transform target, out DamageResult result)
            {
                CallCount++;
                LastIntent = intent;
                LastTarget = target;
                result = new DamageResult(intent.BaseAmount, intent.BaseAmount, false, System.Array.Empty<string>());
                return true;
            }

            public int ApplyAreaDamage(AreaDamageIntent intent)
            {
                return 0;
            }
        }
    }
}
