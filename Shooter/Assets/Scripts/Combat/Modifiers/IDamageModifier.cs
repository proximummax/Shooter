namespace Shooter.Combat
{
    public interface IDamageModifier
    {
        int Priority { get; }
        void Apply(DamageContext context);
    }
}
