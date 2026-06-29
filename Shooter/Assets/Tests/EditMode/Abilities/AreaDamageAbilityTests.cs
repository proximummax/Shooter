using NUnit.Framework;
using Shooter.Abilities;
using Shooter.Combat;
using UnityEngine;

namespace Shooter.Tests.Abilities
{
    public sealed class AreaDamageAbilityTests
    {
        [Test]
        public void Execute_DelegatesAreaDamageIntentToCombatEffectService()
        {
            AreaDamageAbilityEffectDefinition effect = AreaDamageAbilityEffectDefinition.CreateRuntime(
                DamageType.Ability,
                damage: 20f,
                radius: 2f);
            AbilityDefinition definition = AbilityDefinition.CreateRuntime(
                id: "aoe",
                displayName: "AoE Impulse",
                description: "Deals area damage.",
                cooldown: 3f,
                effect);
            var source = new GameObject("source");
            var combatEffects = new RecordingCombatEffectService { AreaDamageResult = 3 };
            int damageLayerMask = 1 << 9;

            try
            {
                source.transform.position = new Vector3(3f, 0f, -2f);
                var ability = new AreaDamageAbility(definition, combatEffects, damageLayerMask);

                int affectedTargets = ability.Execute(new AbilityUseContext(source.transform, Vector3.forward, 0f));

                Assert.That(affectedTargets, Is.EqualTo(3));
                Assert.That(combatEffects.WasAreaDamageCalled, Is.True);
                Assert.That(combatEffects.LastAreaIntent.Center, Is.EqualTo(source.transform.position));
                Assert.That(combatEffects.LastAreaIntent.Radius, Is.EqualTo(2f));
                Assert.That(combatEffects.LastAreaIntent.LayerMask, Is.EqualTo(damageLayerMask));
                Assert.That(combatEffects.LastAreaIntent.Damage.BaseAmount, Is.EqualTo(20f));
                Assert.That(combatEffects.LastAreaIntent.Damage.DamageType, Is.EqualTo(DamageType.Ability));
                Assert.That(combatEffects.LastAreaIntent.Damage.Source, Is.EqualTo(source));
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(definition);
                Object.DestroyImmediate(effect);
            }
        }

        private sealed class RecordingCombatEffectService : ICombatEffectService
        {
            public int AreaDamageResult { get; set; }
            public bool WasAreaDamageCalled { get; private set; }
            public AreaDamageIntent LastAreaIntent { get; private set; }

            public DamageResult ApplyDamage(DamageIntent intent, IDamageable target)
            {
                return new DamageResult(intent.BaseAmount, 0f, false, System.Array.Empty<string>());
            }

            public bool TryApplyDamageToHit(DamageIntent intent, Collider hitCollider, int layerMask, out DamageResult result)
            {
                result = new DamageResult(intent.BaseAmount, 0f, false, System.Array.Empty<string>());
                return false;
            }

            public bool TryApplyDamageToTarget(DamageIntent intent, Transform target, out DamageResult result)
            {
                result = new DamageResult(intent.BaseAmount, 0f, false, System.Array.Empty<string>());
                return false;
            }

            public int ApplyAreaDamage(AreaDamageIntent intent)
            {
                WasAreaDamageCalled = true;
                LastAreaIntent = intent;
                return AreaDamageResult;
            }
        }
    }
}
