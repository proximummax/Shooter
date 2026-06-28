using System.Collections.Generic;

namespace Shooter.Loading
{
    public interface ILoadingService
    {
        IEnumerable<LoadingSnapshot> RunToCompletion();
        IEnumerable<LoadingSnapshot> RetryToCompletion();
        float GetStepDelay(LoadingSnapshot snapshot);
    }
}
