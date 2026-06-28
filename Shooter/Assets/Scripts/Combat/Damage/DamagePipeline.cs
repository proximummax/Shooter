using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shooter.Combat
{
    public sealed class DamagePipeline
    {
        private readonly IReadOnlyList<IDamageModifier> _modifiers;

        public DamagePipeline(IEnumerable<IDamageModifier> modifiers)
        {
            _modifiers = modifiers
                .OrderBy(modifier => modifier.Priority)
                .ToArray();
        }

        public static DamagePipeline Default { get; } = new DamagePipeline(System.Array.Empty<IDamageModifier>());

        public static DamagePipeline Standard { get; } = new DamagePipeline(new IDamageModifier[]
        {
            new ArmorDamageModifier(),
            new CriticalDamageModifier()
        });

        public DamageResult Apply(DamageRequest request)
        {
            var context = new DamageContext(request);

            for (int i = 0; i < _modifiers.Count; i++)
            {
                _modifiers[i].Apply(context);
            }

            float finalAmount = Mathf.Max(0f, context.Amount);
            return new DamageResult(
                rawAmount: request.BaseAmount,
                finalAmount: finalAmount,
                wasCritical: context.WasCritical,
                appliedModifiers: context.AppliedModifiers.ToArray());
        }
    }
}
