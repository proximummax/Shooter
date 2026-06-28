using System;
using NUnit.Framework;
using Shooter.Arena;
using Shooter.Combat;
using Shooter.Composition;
using Shooter.Enemies;
using Shooter.Projectiles;
using UnityEngine;
using VContainer;

namespace Shooter.Tests.Composition
{
    public sealed class ArenaCompositionInstallerTests
    {
        [Test]
        public void Install_BuildsCompositionRootAndResolvesCoreGameplayServices()
        {
            var builder = new ContainerBuilder();
            new ArenaRootInstaller().Install(builder);

            var resolver = builder.Build();
            try
            {
                Assert.DoesNotThrow(() => resolver.Resolve<GameSessionStateStore>());
                Assert.DoesNotThrow(() => resolver.Resolve<IGameSessionStateReader>());
                Assert.DoesNotThrow(() => resolver.Resolve<EnemyRegistry>());
                Assert.DoesNotThrow(() => resolver.Resolve<IGameplayPauseService>());
                Assert.DoesNotThrow(() => resolver.Resolve<IGameRestartService>());
                Assert.DoesNotThrow(() => resolver.Resolve<IDamagePipelineProvider>());
            }
            finally
            {
                (resolver as IDisposable)?.Dispose();
            }

            Time.timeScale = 1f;
        }

        [Test]
        public void PlayerRuntimeBinder_DoesNotOwnAbilityProjectileFactory()
        {
            Type[] dependencyTypes = Array.ConvertAll(
                typeof(PlayerRuntimeBinder).GetConstructors()[0].GetParameters(),
                parameter => parameter.ParameterType);

            Assert.That(Array.IndexOf(dependencyTypes, typeof(IProjectileFactory)), Is.EqualTo(-1));
        }
    }
}
