using Shooter.Arena;
using Shooter.Abilities;
using Shooter.Characters;
using Shooter.Combat;
using Shooter.Player;
using Shooter.Weapons;
using UnityEngine;

namespace Shooter.Composition
{
    public sealed class PlayerRuntimeBinder : IRuntimeSceneBinder
    {
        private readonly CharacterStatsDefinition _playerStats;
        private readonly WeaponDefinition _startingWeapon;
        private readonly PlayerRuntimeContext _player;
        private readonly IGameSessionStateReader _sessionState;
        private readonly IDamagePipelineProvider _damagePipelineProvider;

        public PlayerRuntimeBinder(
            CharacterStatsDefinition playerStats,
            WeaponDefinition startingWeapon,
            PlayerRuntimeContext player,
            IGameSessionStateReader sessionState,
            IDamagePipelineProvider damagePipelineProvider)
        {
            _playerStats = playerStats;
            _startingWeapon = startingWeapon;
            _player = player;
            _sessionState = sessionState;
            _damagePipelineProvider = damagePipelineProvider;
        }

        public void Bind()
        {
            _player.Health.Initialize(_playerStats, _damagePipelineProvider.Combat);
            _player.InputSource.Initialize(_player.GameplayCamera);
            _player.Abilities.Initialize(_startingWeapon.AbilityLoadout, Physics.DefaultRaycastLayers);
            _player.Actor.Initialize(_playerStats, _player.InputSource);
            _player.Actor.BindSessionState(_sessionState);
        }
    }
}
