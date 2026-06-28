using UnityEngine;

namespace Shooter.Combat
{
    public sealed class ArmorDamageModifier : IDamageModifier
    {
        public int Priority => 10;

        public void Apply(DamageContext context)
        {
            if (context.Request.Armor <= 0f)
            {
                return;
            }

            context.Amount = Mathf.Max(0f, context.Amount - context.Request.Armor);
            context.AddModifier("Armor");
        }
    }
}
