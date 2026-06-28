using Shooter.Arena;
using UnityEngine;
using VContainer;

namespace Shooter.Composition
{
    public sealed class SessionFeatureLifetimeScope : FeatureLifetimeScope
    {
        [SerializeField] private GameSessionView _gameSessionView;
        [SerializeField] private ArenaTrigger _arenaTrigger;

        protected override void Configure(IContainerBuilder builder)
        {
            Install(builder);
        }

        public void Install(IContainerBuilder builder)
        {
            new SessionFeatureInstaller(
                SceneReferenceValidator.Require(_gameSessionView, nameof(SessionFeatureLifetimeScope), "Game Session View"),
                SceneReferenceValidator.Require(_arenaTrigger, nameof(SessionFeatureLifetimeScope), "Arena Trigger")).Install(builder);
        }
    }
}
