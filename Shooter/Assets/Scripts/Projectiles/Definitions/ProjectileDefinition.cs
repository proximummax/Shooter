using UnityEngine;

namespace Shooter.Projectiles
{
    [CreateAssetMenu(menuName = "Shooter/Projectile")]
    public sealed class ProjectileDefinition : ScriptableObject
    {
        [SerializeField] private string _id = "projectile";
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private GameObject _hitParticlePrefab;
        [SerializeField] private int _poolPrewarmCount;

        public string Id => _id;
        public GameObject ProjectilePrefab => _projectilePrefab;
        public GameObject HitParticlePrefab => _hitParticlePrefab;
        public int PoolPrewarmCount => Mathf.Max(0, _poolPrewarmCount);

        public static ProjectileDefinition CreateRuntime(
            string id,
            GameObject projectilePrefab,
            GameObject hitParticlePrefab,
            int poolPrewarmCount = 0)
        {
            var definition = CreateInstance<ProjectileDefinition>();
            definition._id = string.IsNullOrWhiteSpace(id) ? "projectile" : id;
            definition._projectilePrefab = projectilePrefab;
            definition._hitParticlePrefab = hitParticlePrefab;
            definition._poolPrewarmCount = Mathf.Max(0, poolPrewarmCount);
            return definition;
        }
    }
}
