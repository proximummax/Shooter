using System;
using Shooter.Abilities;
using Shooter.Arena;
using Shooter.Combat;
using Shooter.Loading;
using Shooter.UI;

namespace Shooter.Composition
{
    public sealed class UiRuntimeBinder : IRuntimeSceneBinder, IDisposable
    {
        private readonly HudView _hud;
        private readonly LoadingView _loading;
        private readonly EndScreenView _endScreen;
        private readonly HealthComponent _playerHealth;
        private readonly AbilityLoadoutComponent _abilityController;
        private readonly GameSessionPresenter _session;
        private readonly IGameRestartService _restartService;
        private readonly ILoadingService _loadingService;
        private LoadingPresenter _loadingPresenter;
        private EndScreenPresenter _endScreenPresenter;
        private bool _isBound;

        public UiRuntimeBinder(
            HudView hud,
            LoadingView loading,
            EndScreenView endScreen,
            HealthComponent playerHealth,
            AbilityLoadoutComponent abilityController,
            GameSessionPresenter session,
            IGameRestartService restartService,
            ILoadingService loadingService)
        {
            _hud = hud;
            _loading = loading;
            _endScreen = endScreen;
            _playerHealth = playerHealth;
            _abilityController = abilityController;
            _session = session;
            _restartService = restartService;
            _loadingService = loadingService;
        }

        public void Bind()
        {
            if (_isBound)
            {
                return;
            }

            new HudPresenter(_hud, _playerHealth, _abilityController).Bind();
            _loadingPresenter = new LoadingPresenter(_loading, _session, _loadingService);
            _endScreenPresenter = new EndScreenPresenter(_session, _endScreen, _restartService);
            _loadingPresenter.StartLoading();
            _isBound = true;
        }

        public void Dispose()
        {
            _loadingPresenter?.Dispose();
            _endScreenPresenter?.Dispose();
        }
    }
}
