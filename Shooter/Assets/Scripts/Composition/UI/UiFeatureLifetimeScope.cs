using Shooter.UI;
using UnityEngine;
using VContainer;

namespace Shooter.Composition
{
    public sealed class UiFeatureLifetimeScope : FeatureLifetimeScope
    {
        [SerializeField] private HudView _hud;
        [SerializeField] private LoadingView _loading;
        [SerializeField] private EndScreenView _endScreen;

        protected override void Configure(IContainerBuilder builder)
        {
            Install(builder);
        }

        public void Install(IContainerBuilder builder)
        {
            new UiFeatureInstaller(
                SceneReferenceValidator.Require(_hud, nameof(UiFeatureLifetimeScope), "HUD"),
                SceneReferenceValidator.Require(_loading, nameof(UiFeatureLifetimeScope), "Loading View"),
                SceneReferenceValidator.Require(_endScreen, nameof(UiFeatureLifetimeScope), "End Screen")).Install(builder);
        }
    }
}
