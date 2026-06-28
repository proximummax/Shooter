using UnityEngine;

namespace Shooter.Abilities
{
    public abstract class AbilityEffectDefinition : ScriptableObject
    {
        public abstract AbilityType Type { get; }
    }
}
