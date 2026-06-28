using NUnit.Framework;
using Shooter.Combat;
using UnityEngine;

namespace Shooter.Tests.Combat
{
    public sealed class HealthComponentTests
    {
        [Test]
        public void ApplyDamage_ClampsHealthAndRaisesDeathOnce()
        {
            var owner = new GameObject("health-owner");
            try
            {
                var health = owner.AddComponent<HealthComponent>();
                health.Initialize(maxHealth: 100f, armor: 5f, new DamagePipeline(new IDamageModifier[] { new ArmorDamageModifier() }));
                int changedCount = 0;
                int deathCount = 0;
                health.HealthChanged += _ => changedCount++;
                health.Died += _ => deathCount++;

                DamageResult firstHit = health.ApplyDamage(new DamageRequest(30f, DamageType.Basic));
                DamageResult lethalHit = health.ApplyDamage(new DamageRequest(200f, DamageType.Basic));
                health.ApplyDamage(new DamageRequest(50f, DamageType.Basic));

                Assert.That(firstHit.FinalAmount, Is.EqualTo(25f));
                Assert.That(lethalHit.FinalAmount, Is.EqualTo(195f));
                Assert.That(health.CurrentHealth, Is.EqualTo(0f));
                Assert.That(health.IsDead, Is.True);
                Assert.That(changedCount, Is.EqualTo(2));
                Assert.That(deathCount, Is.EqualTo(1));
            }
            finally
            {
                Object.DestroyImmediate(owner);
            }
        }
    }
}
