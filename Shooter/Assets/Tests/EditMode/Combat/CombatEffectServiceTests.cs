using NUnit.Framework;
using Shooter.Combat;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shooter.Tests.Combat
{
    public sealed class CombatEffectServiceTests
    {
        [Test]
        public void TryApplyDamageToHit_AppliesDamageThroughTargetHealthPipeline()
        {
            GameObject source = new GameObject("source");
            GameObject target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            HealthComponent health = target.AddComponent<HealthComponent>();
            health.Initialize(100f, 5f, new DamagePipeline(new IDamageModifier[] { new ArmorDamageModifier() }));
            var service = new CombatEffectService();

            try
            {
                bool applied = service.TryApplyDamageToHit(
                    new DamageIntent(20f, DamageType.Basic, source),
                    target.GetComponent<Collider>(),
                    ~0,
                    out DamageResult result);

                Assert.That(applied, Is.True);
                Assert.That(result.FinalAmount, Is.EqualTo(15f));
                Assert.That(health.CurrentHealth, Is.EqualTo(85f));
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void TryApplyDamageToHit_IgnoresSourceCollider()
        {
            GameObject source = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            HealthComponent health = source.AddComponent<HealthComponent>();
            health.Initialize(100f, 0f, DamagePipeline.Default);
            var service = new CombatEffectService();

            try
            {
                bool applied = service.TryApplyDamageToHit(
                    new DamageIntent(20f, DamageType.Basic, source),
                    source.GetComponent<Collider>(),
                    ~0,
                    out DamageResult result);

                Assert.That(applied, Is.False);
                Assert.That(result.FinalAmount, Is.EqualTo(0f));
                Assert.That(health.CurrentHealth, Is.EqualTo(100f));
            }
            finally
            {
                Object.DestroyImmediate(source);
            }
        }

        [Test]
        public void TryApplyDamageToTarget_ResolvesDamageableFromTargetTransform()
        {
            GameObject source = new GameObject("source");
            GameObject target = new GameObject("target");
            HealthComponent health = target.AddComponent<HealthComponent>();
            health.Initialize(100f, 0f, DamagePipeline.Default);
            var service = new CombatEffectService();

            try
            {
                bool applied = service.TryApplyDamageToTarget(
                    new DamageIntent(25f, DamageType.Enemy, source),
                    target.transform,
                    out DamageResult result);

                Assert.That(applied, Is.True);
                Assert.That(result.FinalAmount, Is.EqualTo(25f));
                Assert.That(health.CurrentHealth, Is.EqualTo(75f));
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void ApplyAreaDamage_DamagesOnlyUniqueValidTargetsInsideRadius()
        {
            GameObject source = CreateTarget("source", Vector3.zero);
            GameObject nearTarget = CreateTarget("near-target", new Vector3(1f, 0f, 0f));
            GameObject farTarget = CreateTarget("far-target", new Vector3(5f, 0f, 0f));
            var service = new CombatEffectService();

            try
            {
                Physics.SyncTransforms();

                int affectedTargets = service.ApplyAreaDamage(new AreaDamageIntent(
                    new DamageIntent(20f, DamageType.Ability, source),
                    Vector3.zero,
                    radius: 2f,
                    layerMask: ~0));

                Assert.That(affectedTargets, Is.EqualTo(1));
                Assert.That(source.GetComponent<HealthComponent>().CurrentHealth, Is.EqualTo(100f));
                Assert.That(nearTarget.GetComponent<HealthComponent>().CurrentHealth, Is.EqualTo(80f));
                Assert.That(farTarget.GetComponent<HealthComponent>().CurrentHealth, Is.EqualTo(100f));
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(nearTarget);
                Object.DestroyImmediate(farTarget);
            }
        }

        private static GameObject CreateTarget(string name, Vector3 position)
        {
            GameObject target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            target.name = name;
            target.transform.position = position;
            target.AddComponent<HealthComponent>().Initialize(100f, 0f, DamagePipeline.Default);
            return target;
        }
    }
}
