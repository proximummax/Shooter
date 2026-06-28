using NUnit.Framework;
using Shooter.Abilities;
using Shooter.Combat;
using Shooter.Projectiles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shooter.Tests.Projectiles
{
    public sealed class ProjectileImpactResolverTests
    {
        [Test]
        public void TryResolve_AppliesAbilityDamageToHitHealth()
        {
            GameObject source = new GameObject("source");
            GameObject target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            HealthComponent health = target.AddComponent<HealthComponent>();
            health.Initialize(20f, 0f, DamagePipeline.Default);
            AbilityDefinition ability = AbilityDefinition.CreateRuntime(
                "shot",
                "Shot",
                "Projectile shot.",
                AbilityType.Projectile,
                DamageType.Basic,
                7f,
                0.2f,
                10f,
                0f,
                8f);
            var resolver = new ProjectileImpactResolver(ability, null, source, ~0, null);

            try
            {
                bool resolved = resolver.TryResolve(new ProjectileImpactContext(
                    target.GetComponent<Collider>(),
                    Vector3.zero,
                    Vector3.back));

                Assert.That(resolved, Is.True);
                Assert.That(health.CurrentHealth, Is.EqualTo(13f));
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(target);
                Object.DestroyImmediate(ability);
            }
        }

        [Test]
        public void TryResolve_IgnoresSourceCollider()
        {
            GameObject source = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            AbilityDefinition ability = AbilityDefinition.CreateRuntime(
                "shot",
                "Shot",
                "Projectile shot.",
                AbilityType.Projectile,
                DamageType.Basic,
                7f,
                0.2f,
                10f,
                0f,
                8f);
            var resolver = new ProjectileImpactResolver(ability, null, source, ~0, null);

            try
            {
                bool resolved = resolver.TryResolve(new ProjectileImpactContext(
                    source.GetComponent<Collider>(),
                    Vector3.zero,
                    Vector3.back));

                Assert.That(resolved, Is.False);
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(ability);
            }
        }
    }
}
