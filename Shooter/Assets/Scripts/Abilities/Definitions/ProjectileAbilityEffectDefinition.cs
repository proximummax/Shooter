using Shooter.Combat;
using Shooter.Projectiles;
using UnityEngine;

namespace Shooter.Abilities
{
    [CreateAssetMenu(menuName = "Shooter/Abilities/Effects/Projectile")]
    public sealed class ProjectileAbilityEffectDefinition : AbilityEffectDefinition, IDamageAbilityEffect
    {
        [SerializeField] private DamageType _damageType = DamageType.Basic;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _range = 8f;
        [SerializeField] private float _projectileSpeed = 16f;
        [SerializeField] private ProjectileDefinition _projectile;

        public override AbilityType Type => AbilityType.Projectile;
        public DamageType DamageType => _damageType;
        public float Damage => Mathf.Max(0f, _damage);
        public float Range => Mathf.Max(0f, _range);
        public float ProjectileSpeed => Mathf.Max(0f, _projectileSpeed);
        public ProjectileDefinition Projectile => _projectile;

        public static ProjectileAbilityEffectDefinition CreateRuntime(
            DamageType damageType,
            float damage,
            float range,
            float projectileSpeed,
            ProjectileDefinition projectile = null)
        {
            var definition = CreateInstance<ProjectileAbilityEffectDefinition>();
            definition._damageType = damageType;
            definition._damage = Mathf.Max(0f, damage);
            definition._range = Mathf.Max(0f, range);
            definition._projectileSpeed = Mathf.Max(0f, projectileSpeed);
            definition._projectile = projectile;
            return definition;
        }
    }
}
