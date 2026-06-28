using Shooter.Abilities;
using Shooter.Arena;
using Shooter.Combat;
using Shooter.Loading;
using Shooter.UI;
using VContainer;

namespace Shooter.Composition
{
    public sealed class UiFeatureInstaller
    {
        private readonly HudView _hud;
        private readonly LoadingView _loading;
        private readonly EndScreenView _endScreen;

        public UiFeatureInstaller(HudView hud, LoadingView loading, EndScreenView endScreen)
        {
            _hud = hud;
            _loading = loading;
            _endScreen = endScreen;
        }

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance<ILoadingService>(new DemoLoadingService());

            builder.Register<UiRuntimeBinder>(
                    resolver => new UiRuntimeBinder(
                        _hud,
                        _loading,
                        _endScreen,
                        resolver.Resolve<HealthComponent>(),
                        resolver.Resolve<AbilityLoadoutComponent>(),
                        resolver.Resolve<GameSessionPresenter>(),
                        resolver.Resolve<IGameRestartService>(),
                        resolver.Resolve<ILoadingService>()),
                    Lifetime.Singleton)
                .AsSelf();

            builder.RegisterBuildCallback(resolver => resolver.Resolve<UiRuntimeBinder>().Bind());
        }
    }
}
