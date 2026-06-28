using NUnit.Framework;
using Shooter.Combat;

namespace Shooter.Tests.Combat
{
    public sealed class DamagePipelineTests
    {
        [Test]
        public void Apply_UsesModifiersInPriorityOrder()
        {
            var pipeline = new DamagePipeline(new IDamageModifier[]
            {
                new AddDamageModifier(5f, 20),
                new MultiplyDamageModifier(2f, 10)
            });

            DamageResult result = pipeline.Apply(new DamageRequest(10f, DamageType.Ability));

            Assert.That(result.FinalAmount, Is.EqualTo(25f));
            Assert.That(result.AppliedModifiers, Is.EqualTo(new[] { "multiply", "add" }));
        }

        [Test]
        public void Apply_ArmorAndForcedCriticalShareTheSamePipeline()
        {
            var pipeline = new DamagePipeline(new IDamageModifier[]
            {
                new ArmorDamageModifier(),
                new CriticalDamageModifier()
            });

            DamageResult result = pipeline.Apply(new DamageRequest(
                baseAmount: 50f,
                damageType: DamageType.Basic,
                armor: 10f,
                criticalMultiplier: 2f,
                forceCritical: true));

            Assert.That(result.FinalAmount, Is.EqualTo(80f));
            Assert.That(result.WasCritical, Is.True);
            Assert.That(result.AppliedModifiers, Is.EqualTo(new[] { "Armor", "Critical" }));
        }

        private sealed class MultiplyDamageModifier : IDamageModifier
        {
            private readonly float _multiplier;

            public MultiplyDamageModifier(float multiplier, int priority)
            {
                _multiplier = multiplier;
                Priority = priority;
            }

            public int Priority { get; }

            public void Apply(DamageContext context)
            {
                context.Amount *= _multiplier;
                context.AddModifier("multiply");
            }
        }

        private sealed class AddDamageModifier : IDamageModifier
        {
            private readonly float _amount;

            public AddDamageModifier(float amount, int priority)
            {
                _amount = amount;
                Priority = priority;
            }

            public int Priority { get; }

            public void Apply(DamageContext context)
            {
                context.Amount += _amount;
                context.AddModifier("add");
            }
        }
    }
}
