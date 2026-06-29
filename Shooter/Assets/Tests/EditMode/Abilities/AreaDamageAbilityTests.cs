using NUnit.Framework;
using Shooter.Abilities;
using Shooter.Combat;
using UnityEngine;

namespace Shooter.Tests.Abilities
{
    public sealed class AreaDamageAbilityTests
    {
        [Test]
        public void Execute_DamagesOnlyTargetsInsideRadius()
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
            GameObject nearTarget = CreateTarget("near-target", new Vector3(1f, 0f, 0f));
            GameObject farTarget = CreateTarget("far-target", new Vector3(5f, 0f, 0f));

            try
            {
                Physics.SyncTransforms();
                var ability = new AreaDamageAbility(definition, DamagePipeline.Default, Physics.DefaultRaycastLayers);

                ability.Execute(new AbilityUseContext(source.transform, Vector3.forward, 0f));

                Assert.That(nearTarget.GetComponent<HealthComponent>().CurrentHealth, Is.EqualTo(80f));
                Assert.That(farTarget.GetComponent<HealthComponent>().CurrentHealth, Is.EqualTo(100f));
            }
            finally
            {
                Object.DestroyImmediate(source);
                Object.DestroyImmediate(nearTarget);
                Object.DestroyImmediate(farTarget);
                Object.DestroyImmediate(definition);
                Object.DestroyImmediate(effect);
            }
        }

        private static GameObject CreateTarget(string name, Vector3 position)
        {
            var target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            target.name = name;
            target.transform.position = position;
            target.GetComponent<HealthComponent>()?.Initialize(100f, 0f, DamagePipeline.Default);
            if (target.GetComponent<HealthComponent>() == null)
            {
                target.AddComponent<HealthComponent>().Initialize(100f, 0f, DamagePipeline.Default);
            }

            return target;
        }
    }
}
