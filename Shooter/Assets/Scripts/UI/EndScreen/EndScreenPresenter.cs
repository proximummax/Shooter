using System;
using Shooter.Arena;

namespace Shooter.UI
{
    public sealed class EndScreenPresenter : IDisposable
    {
        private readonly GameSessionPresenter _session;
        private readonly IEndScreenView _view;
        private readonly IGameRestartService _restartService;
        private bool _isDisposed;

        public EndScreenPresenter(
            GameSessionPresenter session,
            IEndScreenView view,
            IGameRestartService restartService)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _restartService = restartService ?? throw new ArgumentNullException(nameof(restartService));

            _session.StateChanged += HandleStateChanged;
            _view.RestartClicked += Restart;
            HandleStateChanged(_session.CurrentState);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _session.StateChanged -= HandleStateChanged;
            _view.RestartClicked -= Restart;
            _isDisposed = true;
        }

        private void HandleStateChanged(GameSessionState state)
        {
            if (state != GameSessionState.Victory && state != GameSessionState.Defeat)
            {
                _view.SetVisible(false);
                return;
            }

            _view.SetTitle(state == GameSessionState.Victory ? "VICTORY" : "DEFEAT");
            _view.SetVisible(true);
        }

        private void Restart()
        {
            _restartService.Restart();
        }
    }
}
