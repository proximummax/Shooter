using Shooter.Player;
using UnityEngine;

namespace Shooter.Arena
{
    [RequireComponent(typeof(Collider))]
    public sealed class ArenaTrigger : MonoBehaviour
    {
        private GameSessionPresenter _session;
        private bool _isUsed;

        public void Initialize(GameSessionPresenter session)
        {
            _session = session;
        }

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isUsed || _session == null || other.GetComponentInParent<PlayerActor>() == null)
            {
                return;
            }

            _isUsed = true;
            _session.BeginArena();
        }
    }
}
