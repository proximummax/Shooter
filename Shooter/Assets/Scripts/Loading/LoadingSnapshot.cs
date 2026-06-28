namespace Shooter.Loading
{
    public readonly struct LoadingSnapshot
    {
        public LoadingSnapshot(float progress, string statusText, LoadingStatus status, bool canRetry)
        {
            Progress = progress;
            StatusText = statusText;
            Status = status;
            CanRetry = canRetry;
        }

        public float Progress { get; }
        public string StatusText { get; }
        public LoadingStatus Status { get; }
        public bool CanRetry { get; }
    }
}
