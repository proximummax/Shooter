using Shooter.Arena;
using Shooter.Combat;
using Shooter.Enemies;
using VContainer;

namespace Shooter.Composition
{
    public sealed class ArenaRootInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            var stateStore = new GameSessionStateStore();
            var enemyRegistry = new EnemyRegistry();
            var pauseService = new TimeScalePauseService();
            var restartService = new SceneRestartService();
            var damagePipelines = new StandardDamagePipelineProvider();
            var combatEffects = new CombatEffectService();

            builder.RegisterInstance(stateStore).AsSelf().As<IGameSessionStateReader>();
            builder.RegisterInstance(enemyRegistry);
            builder.RegisterInstance<IGameplayPauseService>(pauseService);
            builder.RegisterInstance<IGameRestartService>(restartService);
            builder.RegisterInstance<IDamagePipelineProvider>(damagePipelines);
            builder.RegisterInstance<ICombatEffectService>(combatEffects);
        }
    }
}
