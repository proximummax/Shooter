using Shooter.Abilities;
using Shooter.Combat;

namespace Shooter.UI
{
    public interface IHudView
    {
        void Bind(HealthComponent playerHealth, AbilityLoadoutComponent abilityController);
    }
}
