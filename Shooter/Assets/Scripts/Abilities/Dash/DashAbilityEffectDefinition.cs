using UnityEngine;

namespace Shooter.Abilities
{
    [CreateAssetMenu(menuName = "Shooter/Abilities/Effects/Dash")]
    public sealed class DashAbilityEffectDefinition : AbilityEffectDefinition
    {
        [SerializeField] private float _distance = 4f;
        [SerializeField] private float _duration = 0.15f;

        public override AbilityType Type => AbilityType.Dash;
        public float Distance => Mathf.Max(0f, _distance);
        public float Duration => Mathf.Max(0.01f, _duration);

        public static DashAbilityEffectDefinition CreateRuntime(float distance, float duration)
        {
            var definition = CreateInstance<DashAbilityEffectDefinition>();
            definition._distance = Mathf.Max(0f, distance);
            definition._duration = Mathf.Max(0.01f, duration);
            return definition;
        }
    }
}
