using UnityEngine;

namespace Shooter.Arena
{
    public sealed class TimeScalePauseService : IGameplayPauseService
    {
        public void SetPaused(bool isPaused)
        {
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }
}
