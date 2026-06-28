using System;
using UnityEngine;

namespace Shooter.Arena
{
    public sealed class GameSessionModel
    {
        private int _remainingEnemies;

        public event Action<GameSessionState> StateChanged;

        public GameSessionState State { get; private set; } = GameSessionState.Loading;
        public int RemainingEnemies => _remainingEnemies;

        public void CompleteLoading()
        {
            if (State != GameSessionState.Loading)
            {
                return;
            }

            ChangeState(GameSessionState.WaitingForArena);
        }

        public void StartArena(int enemyCount)
        {
            if (State != GameSessionState.WaitingForArena && State != GameSessionState.Loading)
            {
                return;
            }

            _remainingEnemies = Mathf.Max(0, enemyCount);
            ChangeState(_remainingEnemies == 0 ? GameSessionState.Victory : GameSessionState.Running);
        }

        public void NotifyEnemyDefeated()
        {
            if (State != GameSessionState.Running)
            {
                return;
            }

            _remainingEnemies = Mathf.Max(0, _remainingEnemies - 1);
            if (_remainingEnemies == 0)
            {
                ChangeState(GameSessionState.Victory);
            }
        }

        public void NotifyPlayerDied()
        {
            if (State == GameSessionState.Running || State == GameSessionState.WaitingForArena)
            {
                ChangeState(GameSessionState.Defeat);
            }
        }

        private void ChangeState(GameSessionState state)
        {
            if (State == state)
            {
                return;
            }

            State = state;
            StateChanged?.Invoke(State);
        }
    }
}
