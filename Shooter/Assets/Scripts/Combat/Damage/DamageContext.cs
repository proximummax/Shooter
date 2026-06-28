using System.Collections.Generic;

namespace Shooter.Combat
{
    public sealed class DamageContext
    {
        private readonly List<string> _appliedModifiers = new List<string>();

        public DamageContext(DamageRequest request)
        {
            Request = request;
            Amount = request.BaseAmount;
        }

        public DamageRequest Request { get; }
        public float Amount { get; set; }
        public bool WasCritical { get; set; }
        public IReadOnlyList<string> AppliedModifiers => _appliedModifiers;

        public void AddModifier(string modifierName)
        {
            if (!string.IsNullOrWhiteSpace(modifierName))
            {
                _appliedModifiers.Add(modifierName);
            }
        }
    }
}
