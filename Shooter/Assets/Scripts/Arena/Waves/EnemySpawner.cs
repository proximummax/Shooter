using System;
using Shooter.Enemies;
using UnityEngine;

namespace Shooter.Arena
{
    public sealed class EnemySpawner : IEnemyWaveSpawner
    {
        private readonly WaveDefinition _wave;
        private readonly IEnemyFactory _enemyFactory;

        public EnemySpawner(WaveDefinition wave, IEnemyFactory enemyFactory)
        {
            _wave = wave;
            _enemyFactory = enemyFactory;
        }

        public int SpawnWave()
        {
            if (_wave == null)
            {
                return 0;
            }

            int spawned = 0;
            foreach (WaveEntryDefinition entry in _wave.Entries)
            {
                if (entry.Archetype == null)
                {
                    throw new InvalidOperationException("Wave entry must reference an enemy archetype.");
                }

                for (int i = 0; i < entry.Count; i++)
                {
                    float angle = i * Mathf.PI * 2f / Mathf.Max(1, entry.Count);
                    Vector3 position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * entry.SpawnRadius;
                    SpawnEnemy(entry, position, spawned + 1);
                    spawned++;
                }
            }

            return spawned;
        }

        private void SpawnEnemy(WaveEntryDefinition entry, Vector3 position, int index)
        {
            _enemyFactory.Create(entry.Archetype, position, $"{entry.NamePrefix} {index}");
        }
    }
}
