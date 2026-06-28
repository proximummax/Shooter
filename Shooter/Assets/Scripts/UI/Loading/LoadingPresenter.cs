using System;
using System.Collections.Generic;
using Shooter.Arena;
using Shooter.Loading;

namespace Shooter.UI
{
    public sealed class LoadingPresenter : IDisposable
    {
        private readonly ILoadingView _view;
        private readonly GameSessionPresenter _session;
        private readonly ILoadingService _loadingService;
        private bool _isStarted;
        private bool _isDisposed;

        public LoadingPresenter(
            ILoadingView view,
            GameSessionPresenter session,
            ILoadingService loadingService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _loadingService = loadingService ?? throw new ArgumentNullException(nameof(loadingService));
            _view.RetryClicked += Retry;
        }

        public void StartLoading()
        {
            if (_isStarted)
            {
                return;
            }

            _isStarted = true;
            BeginAttempt(_loadingService.RunToCompletion());
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _view.RetryClicked -= Retry;
            _isDisposed = true;
        }

        private void Retry()
        {
            BeginAttempt(_loadingService.RetryToCompletion());
        }

        private void BeginAttempt(IEnumerable<LoadingSnapshot> snapshots)
        {
            _view.SetVisible(true);
            _view.SetRetryVisible(false);
            _view.PlaySnapshots(snapshots, _loadingService.GetStepDelay, Apply);
        }

        private void Apply(LoadingSnapshot snapshot)
        {
            _view.SetProgress(snapshot.Progress);
            _view.SetStatusText(snapshot.StatusText);
            _view.SetRetryVisible(snapshot.CanRetry);

            if (snapshot.Status == LoadingStatus.Succeeded)
            {
                _view.SetVisible(false);
                _session.CompleteLoading();
            }
        }
    }
}
