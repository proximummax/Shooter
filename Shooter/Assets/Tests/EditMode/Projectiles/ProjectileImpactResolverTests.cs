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
        public void TryResolve_DelegatesHitDamageToCombatEffectService()
        {
            GameObject source = new GameObject("source");
            GameObject target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            ProjectileAbilityEffectDefinition effect = ProjectileAbilityEffectDefinition.CreateRuntime(
                DamageType.Basic,
                damage: 7f,
                range: 10f,
                projectileSpeed: 8f);
            var combatEffects = new RecordingCombatEffectService { HitResult = true };
            var resolver = new ProjectileImpactResolver(effect, null, source, ~0, null, combatEffects);

            try
            {
                bool resolved = resolver.TryResolve(new ProjectileImpactContext(
                    target.GetComponent<Collider>(),
                    Vector3.zero,
                    Vector3.back));

                Assert.That(resolved, Is.True);
                Assert.That(combatEffects.WasHitCalled, Is.True);
                Assert.That(combatEffects.LastHitCollider, Is.EqualTo(target.GetComponent<Collider>()));
                Assert.That(combatEffects.LastHitLayerMask, Is.EqualTo(~0));
                Assert.That(combatEffects.LastHitIntent.BaseAmount, Is.EqualTo(7f));
                Assert.That(combatEffects.LastHitIntent.DamageType, Is.EqualTo(DamageType.Basic));
                Assert.That(combatEffects.LastHitIntent.Source, Is.EqualTo(source));
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(target);
                Object.DestroyImmediate(effect);
            }
        }

        [Test]
        public void TryResolve_LeavesTargetFilteringToCombatEffectService()
        {
            GameObject source = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            ProjectileAbilityEffectDefinition effect = ProjectileAbilityEffectDefinition.CreateRuntime(
                DamageType.Basic,
                damage: 7f,
                range: 10f,
                projectileSpeed: 8f);
            var combatEffects = new RecordingCombatEffectService { HitResult = false };
            var resolver = new ProjectileImpactResolver(effect, null, source, ~0, null, combatEffects);

            try
            {
                bool resolved = resolver.TryResolve(new ProjectileImpactContext(
                    source.GetComponent<Collider>(),
                    Vector3.zero,
                    Vector3.back));

                Assert.That(resolved, Is.False);
                Assert.That(combatEffects.WasHitCalled, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(effect);
            }
        }

        private sealed class RecordingCombatEffectService : ICombatEffectService
        {
            public bool HitResult { get; set; }
            public bool WasHitCalled { get; private set; }
            public DamageIntent LastHitIntent { get; private set; }
            public Collider LastHitCollider { get; private set; }
            public int LastHitLayerMask { get; private set; }

            public DamageResult ApplyDamage(DamageIntent intent, IDamageable target)
            {
                return new DamageResult(intent.BaseAmount, 0f, false, System.Array.Empty<string>());
            }

            public bool TryApplyDamageToHit(DamageIntent intent, Collider hitCollider, int layerMask, out DamageResult result)
            {
                WasHitCalled = true;
                LastHitIntent = intent;
                LastHitCollider = hitCollider;
                LastHitLayerMask = layerMask;
                result = new DamageResult(intent.BaseAmount, intent.BaseAmount, false, System.Array.Empty<string>());
                return HitResult;
            }

            public bool TryApplyDamageToTarget(DamageIntent intent, Transform target, out DamageResult result)
            {
                result = new DamageResult(intent.BaseAmount, 0f, false, System.Array.Empty<string>());
                return false;
            }

            public int ApplyAreaDamage(AreaDamageIntent intent)
            {
                return 0;
            }
        }
    }
}
