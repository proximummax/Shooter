using Shooter.Arena;
using Shooter.Combat;
using Shooter.Enemies;
using UnityEngine;
using VContainer;

namespace Shooter.Composition
{
    public sealed class EnemyFeatureInstaller
    {
        private readonly WaveDefinition _wave;
        private readonly Transform _enemyParent;

        public EnemyFeatureInstaller(
            WaveDefinition wave,
            Transform enemyParent)
        {
            _wave = wave;
            _enemyParent = enemyParent;
        }

        public void Install(IContainerBuilder builder)
        {
            builder.Register<EnemyRuntimeContext>(
                    resolver =>
                    {
                        var player = resolver.Resolve<PlayerRuntimeContext>();
                        return new EnemyRuntimeContext(
                            player.Transform,
                            resolver.Resolve<IGameSessionStateReader>(),
                            resolver.Resolve<IDamagePipelineProvider>(),
                            resolver.Resolve<ICombatEffectService>(),
                            player.GameplayCamera);
                    },
                    Lifetime.Singleton)
                .AsSelf();

            builder.Register<EnemyFactory>(
                    resolver =>
                        new EnemyFactory(
                            _enemyParent,
                            resolver.Resolve<EnemyRegistry>(),
                            resolver.Resolve<EnemyRuntimeContext>()),
                    Lifetime.Singleton)
                .AsSelf()
                .As<IEnemyFactory>();

            builder.Register<EnemySpawner>(
                    resolver => new EnemySpawner(_wave, resolver.Resolve<IEnemyFactory>()),
                    Lifetime.Singleton)
                .AsSelf()
                .As<IEnemyWaveSpawner>();
        }
    }
}
