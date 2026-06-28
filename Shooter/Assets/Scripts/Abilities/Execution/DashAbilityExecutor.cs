namespace Shooter.Abilities
{
    public sealed class DashAbilityExecutor : IAbilityExecutor
    {
        public AbilityType AbilityType => AbilityType.Dash;

        public void Execute(AbilityDefinition ability, AbilityUseContext context, int damageLayerMask)
        {
            new DashAbility(ability).Execute(context);
        }
    }
}
