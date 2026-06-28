namespace Shooter.Arena
{
    public interface IGameSessionStateReader
    {
        GameSessionState CurrentState { get; }
    }
}
