using System;
using System.Collections.Generic;
using Shooter.Loading;

namespace Shooter.UI
{
    public interface ILoadingView
    {
        event Action RetryClicked;

        void SetVisible(bool visible);
        void SetProgress(float progress);
        void SetStatusText(string statusText);
        void SetRetryVisible(bool visible);
        void PlaySnapshots(
            IEnumerable<LoadingSnapshot> snapshots,
            Func<LoadingSnapshot, float> delayProvider,
            Action<LoadingSnapshot> onSnapshot);
    }
}
