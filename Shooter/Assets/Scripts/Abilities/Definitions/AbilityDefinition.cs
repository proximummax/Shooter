using System;
using Shooter.Combat;
using Shooter.Projectiles;
using UnityEngine;

namespace Shooter.Abilities
{
    [CreateAssetMenu(menuName = "Shooter/Ability")]
    public sealed class AbilityDefinition : ScriptableObject
    {
        [SerializeField] private string _id = "ability";
        [SerializeField] private string _displayName = "Ability";
        [SerializeField, TextArea] private string _description = "Ability description.";
        [SerializeField] private float _cooldown = 1f;
        [SerializeField] private AbilityEffectDefinition _effect;
        [SerializeField] private Sprite _icon;

        public string Id => _id;
        public string DisplayName => _displayName;
        public string Description => _description;
        public AbilityType Type => _effect != null ? _effect.Type : AbilityType.Projectile;
        public float Cooldown => _cooldown;
        public AbilityEffectDefinition Effect => _effect;
        public DamageType DamageType => DamageEffect != null ? DamageEffect.DamageType : DamageType.Basic;
        public float Damage => DamageEffect != null ? DamageEffect.Damage : 0f;
        public float Range => ProjectileEffect != null ? ProjectileEffect.Range : 0f;
        public float Radius => AreaDamageEffect != null ? AreaDamageEffect.Radius : 0f;
        public float ProjectileSpeed => ProjectileEffect != null ? ProjectileEffect.ProjectileSpeed : 0f;
        public ProjectileDefinition Projectile => ProjectileEffect != null ? ProjectileEffect.Projectile : null;
        public ProjectileAbilityEffectDefinition ProjectileEffect => _effect as ProjectileAbilityEffectDefinition;
        public AreaDamageAbilityEffectDefinition AreaDamageEffect => _effect as AreaDamageAbilityEffectDefinition;
        public DashAbilityEffectDefinition DashEffect => _effect as DashAbilityEffectDefinition;
        public Sprite Icon => _icon;

        private IDamageAbilityEffect DamageEffect => _effect as IDamageAbilityEffect;

        public static AbilityDefinition CreateRuntime(
            string id,
            string displayName,
            string description,
            float cooldown,
            AbilityEffectDefinition effect)
        {
            var definition = CreateInstance<AbilityDefinition>();
            definition._id = id;
            definition._displayName = displayName;
            definition._description = description;
            definition._cooldown = Mathf.Max(0f, cooldown);
            definition._effect = effect;
            return definition;
        }

        public static AbilityDefinition CreateRuntime(
            string id,
            string displayName,
            string description,
            AbilityType type,
            DamageType damageType,
            float damage,
            float cooldown,
            float range,
            float radius,
            float projectileSpeed,
            ProjectileDefinition projectile = null)
        {
            AbilityEffectDefinition effect;
            if (type == AbilityType.Projectile)
            {
                effect = ProjectileAbilityEffectDefinition.CreateRuntime(damageType, damage, range, projectileSpeed, projectile);
            }
            else if (type == AbilityType.AreaDamage)
            {
                effect = AreaDamageAbilityEffectDefinition.CreateRuntime(damageType, damage, radius);
            }
            else
            {
                throw new InvalidOperationException($"Use a typed effect definition to create runtime ability type '{type}'.");
            }

            return CreateRuntime(id, displayName, description, cooldown, effect);
        }
    }
}
