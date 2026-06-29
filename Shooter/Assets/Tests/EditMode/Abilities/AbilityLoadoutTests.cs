using NUnit.Framework;
using Shooter.Abilities;
using Shooter.Combat;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shooter.Tests.Abilities
{
    public sealed class AbilityLoadoutTests
    {
        [Test]
        public void TryUse_ExecutesConfiguredAbilityAndEnforcesCooldownWithoutMonoBehaviour()
        {
            GameObject caster = new GameObject("caster");
            GameObject target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            target.transform.position = Vector3.right;
            HealthComponent targetHealth = target.AddComponent<HealthComponent>();
            targetHealth.Initialize(100f, 0f, DamagePipeline.Default);
            AreaDamageAbilityEffectDefinition effect = AreaDamageAbilityEffectDefinition.CreateRuntime(
                DamageType.Ability,
                damage: 20f,
                radius: 3f);
            AbilityDefinition areaDamage = AbilityDefinition.CreateRuntime(
                "active",
                "Active",
                "Active area attack.",
                cooldown: 2f,
                effect);
            AbilityLoadoutDefinition loadoutDefinition = AbilityLoadoutDefinition.CreateRuntime(
                "starter",
                "Starter",
                new[] { AbilitySlotDefinition.CreateRuntime(AbilitySlotId.Primary, areaDamage) });
            var loadout = new AbilityLoadout();

            try
            {
                Physics.SyncTransforms();
                loadout.ConfigureExecutors(null, new StandardDamagePipelineProvider());
                loadout.Initialize(loadoutDefinition, Physics.DefaultRaycastLayers);

                bool firstUse = loadout.TryUse(AbilitySlotId.Primary, new AbilityUseContext(caster.transform, Vector3.forward, 0f));
                bool blockedByCooldown = loadout.TryUse(AbilitySlotId.Primary, new AbilityUseContext(caster.transform, Vector3.forward, 1f));
                bool secondUse = loadout.TryUse(AbilitySlotId.Primary, new AbilityUseContext(caster.transform, Vector3.forward, 2.01f));

                Assert.That(firstUse, Is.True);
                Assert.That(blockedByCooldown, Is.False);
                Assert.That(secondUse, Is.True);
                Assert.That(targetHealth.CurrentHealth, Is.EqualTo(60f));
            }
            finally
            {
                Object.DestroyImmediate(caster);
                Object.DestroyImmediate(target);
                Object.DestroyImmediate(areaDamage);
                Object.DestroyImmediate(effect);
            }
        }

        [Test]
        public void TryUse_ExecutesDashAbilityFromMobilitySlot()
        {
            GameObject caster = new GameObject("caster");
            DashAbilityEffectDefinition effect = DashAbilityEffectDefinition.CreateRuntime(
                distance: 3f,
                duration: 0.15f);
            AbilityDefinition dash = AbilityDefinition.CreateRuntime(
                "dash",
                "Dash",
                "Short movement burst.",
                cooldown: 1f,
                effect);
            AbilityLoadoutDefinition loadoutDefinition = AbilityLoadoutDefinition.CreateRuntime(
                "mobility",
                "Mobility",
                new[] { AbilitySlotDefinition.CreateRuntime(AbilitySlotId.Mobility, dash) });
            var loadout = new AbilityLoadout();

            try
            {
                loadout.ConfigureExecutors(null, new StandardDamagePipelineProvider());
                loadout.Initialize(loadoutDefinition, Physics.DefaultRaycastLayers);

                bool used = loadout.TryUse(
                    AbilitySlotId.Mobility,
                    new AbilityUseContext(caster.transform, Vector3.forward, Vector3.left, 0f));

                Assert.That(used, Is.True);
                DashMotor motor = caster.GetComponent<DashMotor>();
                Assert.That(motor, Is.Not.Null);

                motor.Tick(0.15f);

                Assert.That(Vector3.Distance(caster.transform.position, Vector3.left * 3f), Is.LessThan(0.001f));
            }
            finally
            {
                Object.DestroyImmediate(caster);
                Object.DestroyImmediate(dash);
                Object.DestroyImmediate(effect);
            }
        }
    }
}
