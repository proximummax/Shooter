namespace Shooter.Arena
{
    public sealed class GameSessionStateStore : IGameSessionStateReader
    {
        public GameSessionState CurrentState { get; private set; } = GameSessionState.Loading;

        internal void SetState(GameSessionState state)
        {
            CurrentState = state;
        }
    }
}
