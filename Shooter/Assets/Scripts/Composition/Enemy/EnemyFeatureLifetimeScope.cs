using Shooter.Arena;
using UnityEngine;
using VContainer;

namespace Shooter.Composition
{
    public sealed class EnemyFeatureLifetimeScope : FeatureLifetimeScope
    {
        [Header("Configs")]
        [SerializeField] private WaveDefinition _wave;

        [Header("Hierarchy")]
        [SerializeField] private Transform _enemyParent;

        protected override void Configure(IContainerBuilder builder)
        {
            Install(builder);
        }

        public void Install(IContainerBuilder builder)
        {
            new EnemyFeatureInstaller(
                RuntimeContentValidator.RequireValidWave(_wave, nameof(EnemyFeatureLifetimeScope)),
                SceneReferenceValidator.Require(_enemyParent, nameof(EnemyFeatureLifetimeScope), "Enemy Parent")).Install(builder);
        }
    }
}
