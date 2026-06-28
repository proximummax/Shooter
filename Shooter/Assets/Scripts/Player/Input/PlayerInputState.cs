using UnityEngine;

namespace Shooter.Player
{
    public readonly struct PlayerInputState
    {
        public PlayerInputState(
            Vector3 moveDirection,
            Vector3 aimDirection,
            bool primaryPressed,
            bool secondaryPressed)
            : this(moveDirection, aimDirection, primaryPressed, secondaryPressed, false)
        {
        }

        public PlayerInputState(
            Vector3 moveDirection,
            Vector3 aimDirection,
            bool primaryPressed,
            bool secondaryPressed,
            bool mobilityPressed)
        {
            MoveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
            AimDirection = aimDirection.sqrMagnitude <= 0.001f ? Vector3.forward : aimDirection.normalized;
            PrimaryPressed = primaryPressed;
            SecondaryPressed = secondaryPressed;
            MobilityPressed = mobilityPressed;
        }

        public Vector3 MoveDirection { get; }
        public Vector3 AimDirection { get; }
        public bool PrimaryPressed { get; }
        public bool SecondaryPressed { get; }
        public bool MobilityPressed { get; }

        public static PlayerInputState Empty { get; } = new PlayerInputState(Vector3.zero, Vector3.forward, false, false, false);
    }
}
