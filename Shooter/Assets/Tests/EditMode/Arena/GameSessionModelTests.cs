using NUnit.Framework;
using Shooter.Arena;

namespace Shooter.Tests.Arena
{
    public sealed class GameSessionModelTests
    {
        [Test]
        public void EnemyDefeated_LastEnemyEndsSessionWithVictory()
        {
            var session = new GameSessionModel();
            session.CompleteLoading();
            session.StartArena(enemyCount: 2);

            session.NotifyEnemyDefeated();
            Assert.That(session.State, Is.EqualTo(GameSessionState.Running));

            session.NotifyEnemyDefeated();
            Assert.That(session.State, Is.EqualTo(GameSessionState.Victory));
        }

        [Test]
        public void PlayerDied_EndsRunningSessionWithDefeat()
        {
            var session = new GameSessionModel();
            session.CompleteLoading();
            session.StartArena(enemyCount: 1);

            session.NotifyPlayerDied();

            Assert.That(session.State, Is.EqualTo(GameSessionState.Defeat));
        }
    }
}
