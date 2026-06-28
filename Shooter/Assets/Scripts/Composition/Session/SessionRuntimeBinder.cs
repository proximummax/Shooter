using Shooter.Arena;

namespace Shooter.Composition
{
    public sealed class SessionRuntimeBinder : IRuntimeSceneBinder
    {
        private readonly GameSessionView _gameSessionView;
        private readonly ArenaTrigger _arenaTrigger;
        private readonly GameSessionPresenter _session;

        public SessionRuntimeBinder(GameSessionView gameSessionView, ArenaTrigger arenaTrigger, GameSessionPresenter session)
        {
            _gameSessionView = gameSessionView;
            _arenaTrigger = arenaTrigger;
            _session = session;
        }

        public void Bind()
        {
            _gameSessionView.Bind(_session);
            _arenaTrigger.Initialize(_session);
        }
    }
}
