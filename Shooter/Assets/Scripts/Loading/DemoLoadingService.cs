using System.Collections.Generic;

namespace Shooter.Loading
{
    public sealed class DemoLoadingService : ILoadingService
    {
        private int _attempts;

        public IEnumerable<LoadingSnapshot> RunToCompletion()
        {
            _attempts++;
            yield return new LoadingSnapshot(0f, "Preparing arena...", LoadingStatus.Running, false);
            yield return new LoadingSnapshot(0.35f, "Loading player config...", LoadingStatus.Running, false);
            yield return new LoadingSnapshot(0.7f, "Preparing enemy wave...", LoadingStatus.Running, false);
            yield return _attempts % 2 == 1
                ? new LoadingSnapshot(1f, "Loading failed. Retry available.", LoadingStatus.Failed, true)
                : new LoadingSnapshot(1f, "Arena ready.", LoadingStatus.Succeeded, false);
        }

        public IEnumerable<LoadingSnapshot> RetryToCompletion()
        {
            return RunToCompletion();
        }

        public float GetStepDelay(LoadingSnapshot snapshot)
        {
            return snapshot.Status == LoadingStatus.Running ? 0.45f : 0f;
        }
    }
}
