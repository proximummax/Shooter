using System;
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
        public Sprite Icon => _icon;

        public TEffect GetEffect<TEffect>() where TEffect : AbilityEffectDefinition
        {
            if (_effect is TEffect typedEffect)
            {
                return typedEffect;
            }

            string actualType = _effect != null ? _effect.GetType().Name : "none";
            throw new InvalidOperationException(
                $"Ability '{_id}' requires effect '{typeof(TEffect).Name}', but has '{actualType}'.");
        }

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
    }
}
