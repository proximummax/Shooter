namespace Shooter.Abilities
{
    public interface IAbilityExecutor
    {
        AbilityType AbilityType { get; }
        void Execute(AbilityDefinition ability, AbilityUseContext context, int damageLayerMask);
    }
}
