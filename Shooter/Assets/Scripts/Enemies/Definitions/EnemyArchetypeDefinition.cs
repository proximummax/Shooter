using Shooter.Characters;
using UnityEngine;

namespace Shooter.Enemies
{
    [CreateAssetMenu(menuName = "Shooter/Enemy Archetype")]
    public sealed class EnemyArchetypeDefinition : ScriptableObject
    {
        [SerializeField] private string _id = "enemy";
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private CharacterStatsDefinition _stats;
        [SerializeField] private int _poolPrewarmCount;

        public string Id => _id;
        public GameObject EnemyPrefab => _enemyPrefab;
        public CharacterStatsDefinition Stats => _stats;
        public int PoolPrewarmCount => Mathf.Max(0, _poolPrewarmCount);

        public static EnemyArchetypeDefinition CreateRuntime(
            string id,
            GameObject enemyPrefab,
            CharacterStatsDefinition stats,
            int poolPrewarmCount = 0)
        {
            var definition = CreateInstance<EnemyArchetypeDefinition>();
            definition._id = string.IsNullOrWhiteSpace(id) ? "enemy" : id;
            definition._enemyPrefab = enemyPrefab;
            definition._stats = stats;
            definition._poolPrewarmCount = Mathf.Max(0, poolPrewarmCount);
            return definition;
        }
    }
}
