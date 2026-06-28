using System;
using System.Collections.Generic;
using System.Linq;
using Shooter.Enemies;
using UnityEngine;

namespace Shooter.Arena
{
    [CreateAssetMenu(menuName = "Shooter/Wave")]
    public sealed class WaveDefinition : ScriptableObject
    {
        [SerializeField] private WaveEntryDefinition[] _entries = Array.Empty<WaveEntryDefinition>();

        public IReadOnlyList<WaveEntryDefinition> Entries => GetEntries();
        public int EnemyCount => GetEntries().Sum(entry => entry.Count);
        public float SpawnRadius => GetEntries().FirstOrDefault()?.SpawnRadius ?? 1f;

        public static WaveDefinition CreateRuntime(
            EnemyArchetypeDefinition archetype,
            int enemyCount,
            float spawnRadius,
            string namePrefix = "Enemy")
        {
            return CreateRuntime(
                WaveEntryDefinition.CreateRuntime(
                    archetype,
                    enemyCount,
                    spawnRadius,
                    namePrefix));
        }

        public static WaveDefinition CreateRuntime(params WaveEntryDefinition[] entries)
        {
            var definition = CreateInstance<WaveDefinition>();
            definition._entries = entries?
                .Where(entry => entry != null && entry.Count > 0)
                .ToArray() ?? Array.Empty<WaveEntryDefinition>();
            return definition;
        }

        private IReadOnlyList<WaveEntryDefinition> GetEntries()
        {
            if (_entries != null && _entries.Length > 0)
            {
                return _entries;
            }

            return Array.Empty<WaveEntryDefinition>();
        }
    }
}
