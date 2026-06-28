using System;
using Shooter.Abilities;
using Shooter.Characters;
using Shooter.Combat;
using Shooter.Player;
using UnityEngine;

namespace Shooter.Composition
{
    public sealed class PlayerRuntimeContext
    {
        public PlayerRuntimeContext(
            Transform transform,
            HealthComponent health,
            PlayerActor actor,
            AbilityLoadoutComponent abilities,
            UnityPlayerInputSource inputSource,
            Camera gameplayCamera)
        {
            Transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));
            Health = health != null ? health : throw new ArgumentNullException(nameof(health));
            Actor = actor != null ? actor : throw new ArgumentNullException(nameof(actor));
            Abilities = abilities != null ? abilities : throw new ArgumentNullException(nameof(abilities));
            InputSource = inputSource != null ? inputSource : throw new ArgumentNullException(nameof(inputSource));
            GameplayCamera = gameplayCamera != null ? gameplayCamera : throw new ArgumentNullException(nameof(gameplayCamera));
        }

        public Transform Transform { get; }
        public HealthComponent Health { get; }
        public PlayerActor Actor { get; }
        public AbilityLoadoutComponent Abilities { get; }
        public UnityPlayerInputSource InputSource { get; }
        public Camera GameplayCamera { get; }
    }
}
