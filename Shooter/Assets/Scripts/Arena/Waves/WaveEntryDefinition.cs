using System;
using Shooter.Enemies;
using UnityEngine;

namespace Shooter.Arena
{
    [Serializable]
    public sealed class WaveEntryDefinition
    {
        [SerializeField] private EnemyArchetypeDefinition _archetype;
        [SerializeField] private int _count = 1;
        [SerializeField] private float _spawnRadius = 7f;
        [SerializeField] private string _namePrefix = "Enemy";

        public EnemyArchetypeDefinition Archetype => _archetype;
        public int Count => Mathf.Max(0, _count);
        public float SpawnRadius => Mathf.Max(1f, _spawnRadius);
        public string NamePrefix => string.IsNullOrWhiteSpace(_namePrefix) ? "Enemy" : _namePrefix;

        public static WaveEntryDefinition CreateRuntime(
            EnemyArchetypeDefinition archetype,
            int count,
            float spawnRadius,
            string namePrefix = "Enemy")
        {
            return new WaveEntryDefinition
            {
                _archetype = archetype,
                _count = count,
                _spawnRadius = spawnRadius,
                _namePrefix = string.IsNullOrWhiteSpace(namePrefix) ? "Enemy" : namePrefix
            };
        }
    }
}
