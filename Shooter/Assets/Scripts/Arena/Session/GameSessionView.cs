using UnityEngine;

namespace Shooter.Arena
{
    public sealed class GameSessionView : MonoBehaviour
    {
        [SerializeField] private string _state;
        [SerializeField] private int _remainingEnemies;
        private GameSessionPresenter _session;

        public void Bind(GameSessionPresenter session)
        {
            if (_session != null)
            {
                _session.StateChanged -= HandleStateChanged;
            }

            _session = session;
            if (_session == null)
            {
                _state = string.Empty;
                _remainingEnemies = 0;
                return;
            }

            _session.StateChanged += HandleStateChanged;
            Apply(_session.CurrentState);
        }

        private void OnDestroy()
        {
            if (_session != null)
            {
                _session.StateChanged -= HandleStateChanged;
            }
        }

        private void HandleStateChanged(GameSessionState state)
        {
            Apply(state);
        }

        private void Apply(GameSessionState state)
        {
            _state = state.ToString();
            _remainingEnemies = _session != null ? _session.RemainingEnemies : 0;
        }
    }
}
