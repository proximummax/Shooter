using NUnit.Framework;
using Shooter.Arena;
using Shooter.Characters;
using Shooter.Player;
using UnityEngine;

namespace Shooter.Tests.Player
{
    public sealed class PlayerPresenterTests
    {
        [Test]
        public void Tick_ReturnsMovementVelocityFromStats()
        {
            CharacterStatsDefinition stats = CharacterStatsDefinition.CreateRuntime(
                "Player",
                10f,
                0f,
                7.5f,
                1f,
                1f,
                1f);
            var input = new StubPlayerInputSource(
                new PlayerInputState(Vector3.right, Vector3.forward, false, false));
            var presenter = new PlayerPresenter();
            presenter.Initialize(stats, input);
            presenter.BindSessionState(new StubSessionState(GameSessionState.Running));

            PlayerFrameCommand frame = presenter.Tick();

            Assert.That(frame.ShouldSimulate, Is.True);
            Assert.That(frame.CanUseAbilities, Is.True);
            Assert.That(frame.MoveVelocity, Is.EqualTo(Vector3.right * 7.5f));
        }

        [Test]
        public void Tick_AllowsMovementButBlocksAbilitiesWhileWaitingForArena()
        {
            CharacterStatsDefinition stats = CharacterStatsDefinition.CreateRuntime(
                "Player",
                10f,
                0f,
                4f,
                1f,
                1f,
                1f);
            var input = new StubPlayerInputSource(
                new PlayerInputState(Vector3.forward, Vector3.right, true, true));
            var presenter = new PlayerPresenter();
            presenter.Initialize(stats, input);
            presenter.BindSessionState(new StubSessionState(GameSessionState.WaitingForArena));

            PlayerFrameCommand frame = presenter.Tick();

            Assert.That(frame.ShouldSimulate, Is.True);
            Assert.That(frame.CanUseAbilities, Is.False);
            Assert.That(frame.PrimaryPressed, Is.False);
            Assert.That(frame.SecondaryPressed, Is.False);
            Assert.That(frame.AimDirection, Is.EqualTo(Vector3.right));
        }

        [Test]
        public void Tick_ExposesMobilityInputForShiftAbilitySlot()
        {
            CharacterStatsDefinition stats = CharacterStatsDefinition.CreateRuntime(
                "Player",
                10f,
                0f,
                4f,
                1f,
                1f,
                1f);
            var input = new StubPlayerInputSource(
                new PlayerInputState(Vector3.forward, Vector3.right, false, false, true));
            var presenter = new PlayerPresenter();
            presenter.Initialize(stats, input);
            presenter.BindSessionState(new StubSessionState(GameSessionState.Running));

            PlayerFrameCommand frame = presenter.Tick();

            Assert.That(frame.MoveDirection, Is.EqualTo(Vector3.forward));
            Assert.That(frame.MobilityPressed, Is.True);
        }

        private sealed class StubPlayerInputSource : IPlayerInputSource
        {
            private readonly PlayerInputState _state;

            public StubPlayerInputSource(PlayerInputState state)
            {
                _state = state;
            }

            public PlayerInputState Read()
            {
                return _state;
            }
        }

        private sealed class StubSessionState : IGameSessionStateReader
        {
            public StubSessionState(GameSessionState state)
            {
                CurrentState = state;
            }

            public GameSessionState CurrentState { get; }
        }
    }
}
