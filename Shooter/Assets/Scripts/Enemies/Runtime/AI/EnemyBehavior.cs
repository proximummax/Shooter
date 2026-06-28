using UnityEngine;

namespace Shooter.Enemies
{
    public sealed class EnemyBehavior
    {
        public EnemyState State { get; private set; } = EnemyState.Idle;

        public void Reset()
        {
            State = EnemyState.Idle;
        }

        public void SetIdle()
        {
            State = EnemyState.Idle;
        }

        public void MarkDead()
        {
            State = EnemyState.Dead;
        }

        public EnemyDecision Evaluate(Vector3 selfPosition, Vector3 targetPosition, float attackRange)
        {
            Vector3 offset = targetPosition - selfPosition;
            offset.y = 0f;

            float sqrMagnitude = offset.sqrMagnitude;
            Vector3 direction = sqrMagnitude <= 0.001f ? Vector3.zero : offset.normalized;
            float attackRangeSqr = attackRange * attackRange;

            State = sqrMagnitude <= attackRangeSqr ? EnemyState.Attack : EnemyState.Chase;
            return new EnemyDecision(State, direction);
        }
    }
}
