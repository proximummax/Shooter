using Shooter.Abilities;
using Shooter.Arena;
using Shooter.Characters;
using Shooter.Combat;
using Shooter.Effects;
using Shooter.Player;
using Shooter.Projectiles;
using Shooter.Weapons;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shooter.Composition
{
    public sealed class PlayerFeatureInstaller
    {
        private readonly CharacterStatsDefinition _playerStats;
        private readonly WeaponDefinition _startingWeapon;
        private readonly Transform _projectilesParent;
        private readonly Transform _particlesParent;
        private readonly PlayerRuntimeContext _player;

        public PlayerFeatureInstaller(
            CharacterStatsDefinition playerStats,
            WeaponDefinition startingWeapon,
            Transform projectilesParent,
            Transform particlesParent,
            PlayerRuntimeContext player)
        {
            _playerStats = playerStats;
            _startingWeapon = startingWeapon;
            _projectilesParent = projectilesParent;
            _particlesParent = particlesParent;
            _player = player;
        }

        public void Install(IContainerBuilder builder)
        {
            var hitParticleFactory = new HitParticleFactory(_particlesParent);
            var projectileFactory = new ProjectileFactory(_projectilesParent, hitParticleFactory);

            builder.RegisterInstance(_player);
            builder.RegisterInstance(_player.Health);
            builder.RegisterComponent(_player.Abilities);
            builder.RegisterInstance<IPlayerInputSource>(_player.InputSource);
            builder.RegisterInstance<IHitParticleFactory>(hitParticleFactory);
            builder.RegisterInstance<IProjectileFactory>(projectileFactory);

            builder.Register<PlayerRuntimeBinder>(
                    resolver => new PlayerRuntimeBinder(
                        _playerStats,
                        _startingWeapon,
                        _player,
                        resolver.Resolve<IGameSessionStateReader>(),
                        resolver.Resolve<IDamagePipelineProvider>()),
                    Lifetime.Singleton)
                .AsSelf();

            builder.RegisterBuildCallback(resolver => resolver.Resolve<PlayerRuntimeBinder>().Bind());
        }
    }
}
