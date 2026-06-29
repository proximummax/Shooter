using Shooter.Arena;
using Shooter.Characters;
using UnityEngine;

namespace Shooter.Player
{
    public sealed class PlayerPresenter
    {
        private const float DefaultMoveSpeed = 6f;

        private CharacterStatsDefinition _stats;
        private IPlayerInputSource _inputSource;
        private IGameSessionStateReader _sessionState;

        public void Initialize(CharacterStatsDefinition stats, IPlayerInputSource inputSource)
        {
            _stats = stats;
            _inputSource = inputSource;
        }

        public void BindSessionState(IGameSessionStateReader sessionState)
        {
            _sessionState = sessionState;
        }

        public PlayerFrameCommand Tick()
        {
            GameSessionState state = _sessionState != null
                ? _sessionState.CurrentState
                : GameSessionState.Running;
            if (state != GameSessionState.WaitingForArena && state != GameSessionState.Running)
            {
                return PlayerFrameCommand.Paused;
            }

            PlayerInputState input = _inputSource != null ? _inputSource.Read() : PlayerInputState.Empty;
            bool canUseAbilities = state == GameSessionState.Running;
            float speed = _stats != null ? _stats.MoveSpeed : DefaultMoveSpeed;
            return new PlayerFrameCommand(
                true,
                canUseAbilities,
                input.MoveDirection * speed,
                input.MoveDirection,
                input.AimDirection,
                input.PrimaryPressed,
                input.SecondaryPressed,
                input.MobilityPressed);
        }
    }
}
