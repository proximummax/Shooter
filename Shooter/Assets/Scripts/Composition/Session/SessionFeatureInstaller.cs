using Shooter.Arena;
using Shooter.Combat;
using Shooter.Enemies;
using VContainer;

namespace Shooter.Composition
{
    public sealed class SessionFeatureInstaller
    {
        private readonly GameSessionView _gameSessionView;
        private readonly ArenaTrigger _arenaTrigger;

        public SessionFeatureInstaller(GameSessionView gameSessionView, ArenaTrigger arenaTrigger)
        {
            _gameSessionView = gameSessionView;
            _arenaTrigger = arenaTrigger;
        }

        public void Install(IContainerBuilder builder)
        {
            builder.Register<GameSessionPresenter>(
                    resolver => new GameSessionPresenter(
                        resolver.Resolve<GameSessionStateStore>(),
                        resolver.Resolve<IEnemyWaveSpawner>(),
                        resolver.Resolve<EnemyRegistry>(),
                        resolver.Resolve<HealthComponent>(),
                        resolver.Resolve<IGameplayPauseService>()),
                    Lifetime.Singleton)
                .AsSelf();

            builder.Register<SessionRuntimeBinder>(
                    resolver => new SessionRuntimeBinder(_gameSessionView, _arenaTrigger, resolver.Resolve<GameSessionPresenter>()),
                    Lifetime.Singleton)
                .AsSelf();

            builder.RegisterBuildCallback(resolver => resolver.Resolve<SessionRuntimeBinder>().Bind());
        }
    }
}
