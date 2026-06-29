using UnityEngine;

namespace Shooter.Player
{
    public readonly struct PlayerFrameCommand
    {
        public PlayerFrameCommand(
            bool shouldSimulate,
            bool canUseAbilities,
            Vector3 moveVelocity,
            Vector3 aimDirection,
            bool primaryPressed,
            bool secondaryPressed)
            : this(
                shouldSimulate,
                canUseAbilities,
                moveVelocity,
                moveVelocity.sqrMagnitude <= 0.001f ? Vector3.zero : moveVelocity.normalized,
                aimDirection,
                primaryPressed,
                secondaryPressed,
                false)
        {
        }

        public PlayerFrameCommand(
            bool shouldSimulate,
            bool canUseAbilities,
            Vector3 moveVelocity,
            Vector3 moveDirection,
            Vector3 aimDirection,
            bool primaryPressed,
            bool secondaryPressed,
            bool mobilityPressed)
        {
            ShouldSimulate = shouldSimulate;
            CanUseAbilities = canUseAbilities;
            MoveVelocity = moveVelocity;
            MoveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
            AimDirection = aimDirection.sqrMagnitude <= 0.001f ? Vector3.forward : aimDirection.normalized;
            PrimaryPressed = canUseAbilities && primaryPressed;
            SecondaryPressed = canUseAbilities && secondaryPressed;
            MobilityPressed = canUseAbilities && mobilityPressed;
        }

        public bool ShouldSimulate { get; }
        public bool CanUseAbilities { get; }
        public Vector3 MoveVelocity { get; }
        public Vector3 MoveDirection { get; }
        public Vector3 AimDirection { get; }
        public bool PrimaryPressed { get; }
        public bool SecondaryPressed { get; }
        public bool MobilityPressed { get; }

        public static PlayerFrameCommand Paused { get; } = new PlayerFrameCommand(
            false,
            false,
            Vector3.zero,
            Vector3.zero,
            Vector3.forward,
            false,
            false,
            false);
    }
}
