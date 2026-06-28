using VContainer.Unity;

namespace Shooter.Composition
{
    public abstract class FeatureLifetimeScope : LifetimeScope
    {
        protected override void Awake()
        {
            EnsureParentReference();
            base.Awake();
        }

        protected virtual void Reset()
        {
            EnsureParentReference();
        }

        private void EnsureParentReference()
        {
            if (parentReference.Object == null && parentReference.Type == null)
            {
                parentReference = ParentReference.Create<ArenaLifetimeScope>();
            }
        }
    }
}
