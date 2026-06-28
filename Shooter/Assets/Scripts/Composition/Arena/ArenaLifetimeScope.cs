using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace Shooter.Composition
{
    public sealed class ArenaLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerFeatureLifetimeScope _playerFeature;
        [SerializeField] private EnemyFeatureLifetimeScope _enemyFeature;
        [SerializeField] private SessionFeatureLifetimeScope _sessionFeature;
        [SerializeField] private UiFeatureLifetimeScope _uiFeature;

        protected override void Configure(IContainerBuilder builder)
        {
            new ArenaRootInstaller().Install(builder);
            SceneReferenceValidator.Require(_playerFeature, nameof(ArenaLifetimeScope), "Player Feature").Install(builder);
            SceneReferenceValidator.Require(_enemyFeature, nameof(ArenaLifetimeScope), "Enemy Feature").Install(builder);
            SceneReferenceValidator.Require(_sessionFeature, nameof(ArenaLifetimeScope), "Session Feature").Install(builder);
            SceneReferenceValidator.Require(_uiFeature, nameof(ArenaLifetimeScope), "UI Feature").Install(builder);
        }
    }
}
