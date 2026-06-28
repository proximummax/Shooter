using NUnit.Framework;
using Shooter.Enemies;
using UnityEngine;

namespace Shooter.Tests.Enemies
{
    public sealed class EnemyBehaviorTests
    {
        [Test]
        public void Evaluate_ReturnsChaseOutsideAttackRangeAndAttackInsideRange()
        {
            var behavior = new EnemyBehavior();

            EnemyDecision chase = behavior.Evaluate(Vector3.zero, new Vector3(3f, 0f, 0f), 1f);
            EnemyDecision attack = behavior.Evaluate(Vector3.zero, new Vector3(0.5f, 0f, 0f), 1f);

            Assert.That(chase.State, Is.EqualTo(EnemyState.Chase));
            Assert.That(chase.Direction, Is.EqualTo(Vector3.right));
            Assert.That(attack.State, Is.EqualTo(EnemyState.Attack));
            Assert.That(behavior.State, Is.EqualTo(EnemyState.Attack));
        }
    }
}
