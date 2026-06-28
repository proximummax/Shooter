namespace Shooter.Combat
{
    public sealed class StandardDamagePipelineProvider : IDamagePipelineProvider
    {
        public DamagePipeline Combat => DamagePipeline.Standard;
    }
}
