using Shooter.Combat;
using UnityEngine;

namespace Shooter.Abilities
{
    [CreateAssetMenu(menuName = "Shooter/Abilities/Effects/Area Damage")]
    public sealed class AreaDamageAbilityEffectDefinition : AbilityEffectDefinition, IDamageAbilityEffect
    {
        [SerializeField] private DamageType _damageType = DamageType.Ability;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _radius = 2f;

        public override AbilityType Type => AbilityType.AreaDamage;
        public DamageType DamageType => _damageType;
        public float Damage => Mathf.Max(0f, _damage);
        public float Radius => Mathf.Max(0f, _radius);

        public static AreaDamageAbilityEffectDefinition CreateRuntime(
            DamageType damageType,
            float damage,
            float radius)
        {
            var definition = CreateInstance<AreaDamageAbilityEffectDefinition>();
            definition._damageType = damageType;
            definition._damage = Mathf.Max(0f, damage);
            definition._radius = Mathf.Max(0f, radius);
            return definition;
        }
    }
}
