using Shooter.Combat;

namespace Shooter.Abilities
{
    public interface IDamageAbilityEffect
    {
        DamageType DamageType { get; }
        float Damage { get; }
    }
}
