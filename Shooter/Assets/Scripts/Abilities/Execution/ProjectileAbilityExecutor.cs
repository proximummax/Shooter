using Shooter.Projectiles;

namespace Shooter.Abilities
{
    public sealed class ProjectileAbilityExecutor : IAbilityExecutor
    {
        private readonly IProjectileFactory _projectileFactory;

        public ProjectileAbilityExecutor(IProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }

        public AbilityType AbilityType => AbilityType.Projectile;

        public void Execute(AbilityDefinition ability, AbilityUseContext context, int damageLayerMask)
        {
            new ProjectileAbility(ability, _projectileFactory, damageLayerMask).Execute(context);
        }
    }
}
