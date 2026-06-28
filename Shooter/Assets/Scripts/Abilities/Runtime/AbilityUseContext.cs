using UnityEngine;

namespace Shooter.Abilities
{
    public readonly struct AbilityUseContext
    {
        public AbilityUseContext(Transform caster, Vector3 aimDirection, float time)
            : this(caster, aimDirection, Vector3.zero, time)
        {
        }

        public AbilityUseContext(Transform caster, Vector3 aimDirection, Vector3 moveDirection, float time)
        {
            Caster = caster;
            AimDirection = aimDirection.sqrMagnitude <= 0.001f ? Vector3.forward : aimDirection.normalized;
            MoveDirection = moveDirection.sqrMagnitude <= 0.001f ? Vector3.zero : moveDirection.normalized;
            Time = time;
        }

        public Transform Caster { get; }
        public Vector3 AimDirection { get; }
        public Vector3 MoveDirection { get; }
        public float Time { get; }
    }
}
