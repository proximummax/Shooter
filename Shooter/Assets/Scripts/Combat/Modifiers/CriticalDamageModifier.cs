namespace Shooter.Combat
{
    public sealed class CriticalDamageModifier : IDamageModifier
    {
        public int Priority => 20;

        public void Apply(DamageContext context)
        {
            if (!context.Request.ForceCritical || context.Request.CriticalMultiplier <= 1f)
            {
                return;
            }

            context.Amount *= context.Request.CriticalMultiplier;
            context.WasCritical = true;
            context.AddModifier("Critical");
        }
    }
}
