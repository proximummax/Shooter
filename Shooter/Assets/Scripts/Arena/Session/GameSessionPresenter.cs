using System;
using Shooter.Combat;
using Shooter.Enemies;
using UnityEngine;

namespace Shooter.Arena
{
    public sealed class GameSessionPresenter : IDisposable
    {
        private readonly GameSessionStateStore _stateStore;
        private readonly IEnemyWaveSpawner _enemySpawner;
        private readonly EnemyRegistry _enemyRegistry;
        private readonly HealthComponent _playerHealth;
        private readonly IGameplayPauseService _pauseService;
        private readonly GameSessionModel _model = new GameSessionModel();
        private bool _arenaStarted;
        private bool _isDisposed;

        public event Action<GameSessionState> StateChanged;

        public GameSessionState CurrentState => _model.State;
        public int RemainingEnemies => _model.RemainingEnemies;

        public GameSessionPresenter(
            IEnemyWaveSpawner enemySpawner,
            EnemyRegistry enemyRegistry,
            HealthComponent playerHealth)
            : this(new GameSessionStateStore(), enemySpawner, enemyRegistry, playerHealth, new TimeScalePauseService())
        {
        }

        public GameSessionPresenter(
            GameSessionStateStore stateStore,
            IEnemyWaveSpawner enemySpawner,
            EnemyRegistry enemyRegistry,
            HealthComponent playerHealth)
            : this(stateStore, enemySpawner, enemyRegistry, playerHealth, new TimeScalePauseService())
        {
        }

        public GameSessionPresenter(
            GameSessionStateStore stateStore,
            IEnemyWaveSpawner enemySpawner,
            EnemyRegistry enemyRegistry,
            HealthComponent playerHealth,
            IGameplayPauseService pauseService)
        {
            _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
            _enemySpawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
            _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));
            _playerHealth = playerHealth ?? throw new ArgumentNullException(nameof(playerHealth));
            _pauseService = pauseService ?? throw new ArgumentNullException(nameof(pauseService));

            _model.StateChanged += HandleModelStateChanged;
            _enemyRegistry.EnemyDefeated += HandleEnemyDefeated;
            _playerHealth.Died += HandlePlayerDied;
            _stateStore.SetState(CurrentState);
            _pauseService.SetPaused(true);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _model.StateChanged -= HandleModelStateChanged;
            _enemyRegistry.EnemyDefeated -= HandleEnemyDefeated;
            _playerHealth.Died -= HandlePlayerDied;
            _isDisposed = true;
        }

        public void CompleteLoading()
        {
            _model.CompleteLoading();
        }

        public void BeginArena()
        {
            if (_arenaStarted || CurrentState != GameSessionState.WaitingForArena)
            {
                return;
            }

            _arenaStarted = true;
            int enemyCount = _enemySpawner.SpawnWave();
            _model.StartArena(enemyCount);
        }

        private void HandleEnemyDefeated(HealthComponent health)
        {
            _model.NotifyEnemyDefeated();
        }

        private void HandlePlayerDied(HealthComponent health)
        {
            _model.NotifyPlayerDied();
        }

        private void HandleModelStateChanged(GameSessionState state)
        {
            bool isPaused = state == GameSessionState.Loading || state == GameSessionState.Victory || state == GameSessionState.Defeat;
            _pauseService.SetPaused(isPaused);
            _stateStore.SetState(state);
            StateChanged?.Invoke(state);
        }
    }
}
