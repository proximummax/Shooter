using NUnit.Framework;
using Shooter.Abilities;
using Shooter.Arena;
using Shooter.Characters;
using Shooter.Combat;
using Shooter.Effects;
using Shooter.Enemies;
using Shooter.Projectiles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shooter.Tests.Factories
{
    public sealed class FactoryPoolTests
    {
        [Test]
        public void ProjectileFactory_ParentsProjectileAndReusesReturnedInstance()
        {
            GameObject source = new GameObject("source");
            Transform parent = new GameObject("ProjectilesParent").transform;
            GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            prefab.AddComponent<Projectile>();
            GameObject hitPrefab = new GameObject("hit-effect");
            hitPrefab.AddComponent<ParticleSystem>();
            hitPrefab.AddComponent<HitParticleEffect>();
            Transform particlesParent = new GameObject("ParticlesParent").transform;

            try
            {
                ProjectileDefinition projectileDefinition = ProjectileDefinition.CreateRuntime(
                    id: "default-projectile",
                    projectilePrefab: prefab,
                    hitParticlePrefab: hitPrefab);
                ProjectileAbilityEffectDefinition effect = ProjectileAbilityEffectDefinition.CreateRuntime(
                    DamageType.Basic,
                    damage: 10f,
                    range: 10f,
                    projectileSpeed: 8f,
                    projectileDefinition);
                AbilityDefinition definition = AbilityDefinition.CreateRuntime(
                    "basic",
                    "Basic Shot",
                    "Projectile.",
                    cooldown: 0.2f,
                    effect);
                var hitFactory = new HitParticleFactory(particlesParent);
                var factory = new ProjectileFactory(parent, hitFactory, new CombatEffectService());

                Projectile first = factory.Create(definition, source, Vector3.forward, ~0, Vector3.zero);
                first.Release();
                Projectile second = factory.Create(definition, source, Vector3.forward, ~0, Vector3.one);

                Assert.That(second, Is.SameAs(first));
                Assert.That(second.transform.parent, Is.SameAs(parent));
                Assert.That(second.gameObject.activeSelf, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(parent.gameObject);
                Object.DestroyImmediate(particlesParent.gameObject);
                Object.DestroyImmediate(prefab);
                Object.DestroyImmediate(hitPrefab);
            }
        }

        [Test]
        public void ProjectileFactory_UsesSeparatePoolsPerProjectileDefinition()
        {
            GameObject source = new GameObject("source");
            Transform parent = new GameObject("ProjectilesParent").transform;
            Transform particlesParent = new GameObject("ParticlesParent").transform;
            GameObject firstPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            firstPrefab.name = "first-prefab";
            firstPrefab.AddComponent<Projectile>();
            GameObject secondPrefab = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            secondPrefab.name = "second-prefab";
            secondPrefab.AddComponent<Projectile>();
            GameObject hitPrefab = new GameObject("hit-effect");
            hitPrefab.AddComponent<ParticleSystem>();
            hitPrefab.AddComponent<HitParticleEffect>();

            try
            {
                ProjectileDefinition firstProjectile = ProjectileDefinition.CreateRuntime("first", firstPrefab, hitPrefab);
                ProjectileDefinition secondProjectile = ProjectileDefinition.CreateRuntime("second", secondPrefab, hitPrefab);
                ProjectileAbilityEffectDefinition firstEffect = ProjectileAbilityEffectDefinition.CreateRuntime(
                    DamageType.Basic,
                    damage: 10f,
                    range: 10f,
                    projectileSpeed: 8f,
                    firstProjectile);
                ProjectileAbilityEffectDefinition secondEffect = ProjectileAbilityEffectDefinition.CreateRuntime(
                    DamageType.Basic,
                    damage: 10f,
                    range: 10f,
                    projectileSpeed: 8f,
                    secondProjectile);
                AbilityDefinition firstAbility = AbilityDefinition.CreateRuntime(
                    "first-shot",
                    "First Shot",
                    "Projectile.",
                    cooldown: 0.2f,
                    firstEffect);
                AbilityDefinition secondAbility = AbilityDefinition.CreateRuntime(
                    "second-shot",
                    "Second Shot",
                    "Projectile.",
                    cooldown: 0.2f,
                    secondEffect);
                var factory = new ProjectileFactory(parent, new HitParticleFactory(particlesParent), new CombatEffectService());

                Projectile first = factory.Create(firstAbility, source, Vector3.forward, ~0, Vector3.zero);
                Projectile second = factory.Create(secondAbility, source, Vector3.forward, ~0, Vector3.one);

                Assert.That(first, Is.Not.SameAs(second));
                Assert.That(first.name, Does.Contain("first-prefab"));
                Assert.That(second.name, Does.Contain("second-prefab"));
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(parent.gameObject);
                Object.DestroyImmediate(particlesParent.gameObject);
                Object.DestroyImmediate(firstPrefab);
                Object.DestroyImmediate(secondPrefab);
                Object.DestroyImmediate(hitPrefab);
            }
        }

        [Test]
        public void EnemyFactory_ReturnsDeadEnemyToPoolAndReusesInstance()
        {
            GameObject target = new GameObject("player");
            Transform parent = new GameObject("EnemyParent").transform;
            GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            prefab.AddComponent<HealthComponent>();
            prefab.AddComponent<EnemyActor>();
            prefab.AddComponent<PooledEnemy>();
            var registry = new EnemyRegistry();

            try
            {
                CharacterStatsDefinition stats = CharacterStatsDefinition.CreateRuntime(
                    displayName: "Enemy",
                    maxHealth: 10f,
                    armor: 0f,
                    moveSpeed: 1f,
                    attackRange: 1f,
                    attackDamage: 1f,
                    attackCooldown: 1f);
                EnemyArchetypeDefinition archetype = EnemyArchetypeDefinition.CreateRuntime("enemy", prefab, stats);
                var runtimeContext = new EnemyRuntimeContext(
                    target.transform,
                    new GameSessionStateStore(),
                    new StandardDamagePipelineProvider(),
                    new CombatEffectService());
                var factory = new EnemyFactory(
                    parent,
                    registry,
                    runtimeContext);

                PooledEnemy first = factory.Create(archetype, Vector3.zero, "Enemy 1");
                first.Health.ApplyDamage(new DamageRequest(50f, DamageType.Basic, target, first.Health));
                PooledEnemy second = factory.Create(archetype, Vector3.one, "Enemy 2");

                Assert.That(second, Is.SameAs(first));
                Assert.That(second.transform.parent, Is.SameAs(parent));
                Assert.That(second.gameObject.activeSelf, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(target);
                Object.DestroyImmediate(parent.gameObject);
                Object.DestroyImmediate(prefab);
            }
        }

        [Test]
        public void HitParticleFactory_ParentsEffectAndReusesReturnedInstance()
        {
            Transform parent = new GameObject("ParticlesParent").transform;
            GameObject prefab = new GameObject("hit-effect");
            prefab.AddComponent<ParticleSystem>();
            prefab.AddComponent<HitParticleEffect>();
            GameObject projectilePrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            try
            {
                ProjectileDefinition projectileDefinition = ProjectileDefinition.CreateRuntime("projectile", projectilePrefab, prefab);
                var factory = new HitParticleFactory(parent);

                HitParticleEffect first = factory.Play(projectileDefinition.HitParticlePrefab, Vector3.zero, Vector3.forward);
                first.Release();
                HitParticleEffect second = factory.Play(projectileDefinition.HitParticlePrefab, Vector3.one, Vector3.back);

                Assert.That(second, Is.SameAs(first));
                Assert.That(second.transform.parent, Is.SameAs(parent));
                Assert.That(second.gameObject.activeSelf, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(parent.gameObject);
                Object.DestroyImmediate(prefab);
                Object.DestroyImmediate(projectilePrefab);
            }
        }
    }
}
