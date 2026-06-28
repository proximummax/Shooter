using Shooter.Combat;

namespace Shooter.Abilities
{
    public sealed class AreaDamageAbilityExecutor : IAbilityExecutor
    {
        private readonly IDamagePipelineProvider _damagePipelineProvider;

        public AreaDamageAbilityExecutor(IDamagePipelineProvider damagePipelineProvider)
        {
            _damagePipelineProvider = damagePipelineProvider;
        }

        public AbilityType AbilityType => AbilityType.AreaDamage;

        public void Execute(AbilityDefinition ability, AbilityUseContext context, int damageLayerMask)
        {
            DamagePipeline pipeline = _damagePipelineProvider != null ? _damagePipelineProvider.Combat : DamagePipeline.Default;
            new AreaDamageAbility(ability, pipeline, damageLayerMask).Execute(context);
        }
    }
}
