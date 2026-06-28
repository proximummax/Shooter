using Shooter.Abilities;
using Shooter.Characters;
using Shooter.Combat;
using Shooter.Player;
using Shooter.Weapons;
using UnityEngine;
using VContainer;

namespace Shooter.Composition
{
    public sealed class PlayerFeatureLifetimeScope : FeatureLifetimeScope
    {
        [Header("Configs")]
        [SerializeField] private CharacterStatsDefinition _playerStats;
        [SerializeField] private WeaponDefinition _startingWeapon;

        [Header("Hierarchy")]
        [SerializeField] private Transform _projectilesParent;
        [SerializeField] private Transform _particlesParent;

        [Header("Scene")]
        [SerializeField] private Transform _player;
        [SerializeField] private Camera _gameplayCamera;

        protected override void Configure(IContainerBuilder builder)
        {
            Install(builder);
        }

        public void Install(IContainerBuilder builder)
        {
            Transform player = SceneReferenceValidator.Require(_player, nameof(PlayerFeatureLifetimeScope), "Player");
            var playerContext = new PlayerRuntimeContext(
                player,
                SceneReferenceValidator.RequireComponent<HealthComponent>(player, nameof(PlayerFeatureLifetimeScope), "Player"),
                SceneReferenceValidator.RequireComponent<PlayerActor>(player, nameof(PlayerFeatureLifetimeScope), "Player"),
                SceneReferenceValidator.RequireComponent<AbilityLoadoutComponent>(player, nameof(PlayerFeatureLifetimeScope), "Player"),
                SceneReferenceValidator.RequireComponent<UnityPlayerInputSource>(player, nameof(PlayerFeatureLifetimeScope), "Player"),
                SceneReferenceValidator.Require(_gameplayCamera, nameof(PlayerFeatureLifetimeScope), "Gameplay Camera"));
            new PlayerFeatureInstaller(
                SceneReferenceValidator.Require(_playerStats, nameof(PlayerFeatureLifetimeScope), "Player Stats"),
                RuntimeContentValidator.RequireValidWeapon(_startingWeapon, nameof(PlayerFeatureLifetimeScope)),
                SceneReferenceValidator.Require(_projectilesParent, nameof(PlayerFeatureLifetimeScope), "Projectiles Parent"),
                SceneReferenceValidator.Require(_particlesParent, nameof(PlayerFeatureLifetimeScope), "Particles Parent"),
                playerContext).Install(builder);
        }
    }
}
