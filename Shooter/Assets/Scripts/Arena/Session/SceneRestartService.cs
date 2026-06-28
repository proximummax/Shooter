using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shooter.Arena
{
    public sealed class SceneRestartService : IGameRestartService
    {
        public void Restart()
        {
            Time.timeScale = 1f;
            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.buildIndex >= 0)
            {
                SceneManager.LoadScene(activeScene.buildIndex);
                return;
            }

            if (!string.IsNullOrEmpty(activeScene.name))
            {
                SceneManager.LoadScene(activeScene.name);
                return;
            }

            SceneManager.LoadScene(0);
        }
    }
}
