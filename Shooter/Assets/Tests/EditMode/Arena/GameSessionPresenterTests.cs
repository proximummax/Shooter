using NUnit.Framework;
using Shooter.Arena;
using Shooter.Combat;
using Shooter.Enemies;
using UnityEngine;

namespace Shooter.Tests.Arena
{
    public sealed class GameSessionPresenterTests
    {
        [Test]
        public void BeginArena_UsesInjectedSpawnerAndSwitchesToRunning()
        {
            GameObject playerObject = new GameObject("player");
            var spawner = new CountingSpawner { EnemyCount = 3 };
            var pauseService = new CountingPauseService();
            GameSessionPresenter session = null;

            try
            {
                var playerHealth = playerObject.AddComponent<HealthComponent>();
                playerHealth.Initialize(10f, 0f, DamagePipeline.Default);
                session = new GameSessionPresenter(
                    new GameSessionStateStore(),
                    spawner,
                    new EnemyRegistry(),
                    playerHealth,
                    pauseService);

                session.CompleteLoading();
                session.BeginArena();

                Assert.That(spawner.SpawnCalls, Is.EqualTo(1));
                Assert.That(session.CurrentState, Is.EqualTo(GameSessionState.Running));
                Assert.That(session.RemainingEnemies, Is.EqualTo(3));
                Assert.That(pauseService.LastPausedValue, Is.False);

            }
            finally
            {
                session?.Dispose();
                Object.DestroyImmediate(playerObject);
            }
        }

        [Test]
        public void PlayerDeath_EndsSessionWithDefeat()
        {
            GameObject playerObject = new GameObject("player");
            var spawner = new CountingSpawner { EnemyCount = 1 };
            var pauseService = new CountingPauseService();
            GameSessionPresenter session = null;

            try
            {
                var playerHealth = playerObject.AddComponent<HealthComponent>();
                playerHealth.Initialize(10f, 0f, DamagePipeline.Default);
                session = new GameSessionPresenter(
                    new GameSessionStateStore(),
                    spawner,
                    new EnemyRegistry(),
                    playerHealth,
                    pauseService);

                session.CompleteLoading();
                session.BeginArena();
                playerHealth.ApplyDamage(new DamageRequest(20f, DamageType.Enemy, playerObject, playerHealth));

                Assert.That(session.CurrentState, Is.EqualTo(GameSessionState.Defeat));
                Assert.That(pauseService.LastPausedValue, Is.True);

            }
            finally
            {
                session?.Dispose();
                Object.DestroyImmediate(playerObject);
            }
        }

        private sealed class CountingSpawner : IEnemyWaveSpawner
        {
            public int EnemyCount { get; set; }
            public int SpawnCalls { get; private set; }

            public int SpawnWave()
            {
                SpawnCalls++;
                return EnemyCount;
            }
        }

        private sealed class CountingPauseService : IGameplayPauseService
        {
            public bool? LastPausedValue { get; private set; }

            public void SetPaused(bool isPaused)
            {
                LastPausedValue = isPaused;
            }
        }
    }
}
