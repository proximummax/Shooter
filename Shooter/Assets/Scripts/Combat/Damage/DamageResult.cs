using System.Collections.Generic;

namespace Shooter.Combat
{
    public readonly struct DamageResult
    {
        public DamageResult(float rawAmount, float finalAmount, bool wasCritical, IReadOnlyList<string> appliedModifiers)
        {
            RawAmount = rawAmount;
            FinalAmount = finalAmount;
            WasCritical = wasCritical;
            AppliedModifiers = appliedModifiers;
        }

        public float RawAmount { get; }
        public float FinalAmount { get; }
        public bool WasCritical { get; }
        public IReadOnlyList<string> AppliedModifiers { get; }
    }
}
