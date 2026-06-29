using System;
using System.Collections.Generic;
using Shooter.Combat;

namespace Shooter.Enemies
{
    public sealed class EnemyRegistry
    {
        private readonly HashSet<HealthComponent> _enemies = new HashSet<HealthComponent>();

        public event Action<HealthComponent> EnemyRegistered;
        public event Action<HealthComponent> EnemyDefeated;
        public event Action AllEnemiesDefeated;

        public void Register(HealthComponent health)
        {
            if (health == null || !_enemies.Add(health))
            {
                return;
            }

            health.Died += HandleEnemyDied;
            EnemyRegistered?.Invoke(health);
        }

        public void Unregister(HealthComponent health)
        {
            if (health == null || !_enemies.Remove(health))
            {
                return;
            }

            health.Died -= HandleEnemyDied;
            EnemyDefeated?.Invoke(health);
            if (_enemies.Count == 0)
            {
                AllEnemiesDefeated?.Invoke();
            }
        }

        private void HandleEnemyDied(HealthComponent health)
        {
            Unregister(health);
        }
    }
}
