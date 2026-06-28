using Shooter.Abilities;
using Shooter.Arena;
using Shooter.Characters;
using UnityEngine;

namespace Shooter.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AbilityLoadoutComponent))]
    [RequireComponent(typeof(UnityPlayerInputSource))]
    public sealed class PlayerActor : MonoBehaviour
    {
        private readonly PlayerPresenter _presenter = new PlayerPresenter();
        private CharacterController _characterController;
        private AbilityLoadoutComponent _abilityLoadout;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _abilityLoadout = GetComponent<AbilityLoadoutComponent>();
            _presenter.Initialize(null, GetComponent<UnityPlayerInputSource>());
        }

        public void Initialize(CharacterStatsDefinition stats, IPlayerInputSource inputSource)
        {
            _presenter.Initialize(stats, inputSource);
        }

        public void BindSessionState(IGameSessionStateReader sessionState)
        {
            _presenter.BindSessionState(sessionState);
        }

        private void Update()
        {
            PlayerFrameCommand frame = _presenter.Tick();
            if (!frame.ShouldSimulate)
            {
                return;
            }

            _characterController.SimpleMove(frame.MoveVelocity);
            transform.rotation = Quaternion.LookRotation(frame.AimDirection);

            if (!frame.CanUseAbilities)
            {
                return;
            }

            if (frame.PrimaryPressed)
            {
                _abilityLoadout.TryUse(AbilitySlotId.Primary, frame.AimDirection, frame.MoveDirection, Time.time);
            }

            if (frame.SecondaryPressed)
            {
                _abilityLoadout.TryUse(AbilitySlotId.Secondary, frame.AimDirection, frame.MoveDirection, Time.time);
            }

            if (frame.MobilityPressed)
            {
                _abilityLoadout.TryUse(AbilitySlotId.Mobility, frame.AimDirection, frame.MoveDirection, Time.time);
            }
        }
    }
}
