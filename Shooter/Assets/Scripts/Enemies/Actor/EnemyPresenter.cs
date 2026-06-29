using UnityEngine;

namespace Shooter.Enemies
{
    public sealed class EnemyPresenter
    {
        private readonly Collider[] _colliders;

        public EnemyPresenter(GameObject owner)
        {
            _colliders = owner.GetComponentsInChildren<Collider>(true);
        }

        public void Restore()
        {
            SetCollidersEnabled(true);
        }

        public void ShowDead()
        {
            SetCollidersEnabled(false);
        }

        private void SetCollidersEnabled(bool enabled)
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = enabled;
            }
        }
    }
}
