using UnityEngine;

namespace Shooter.Enemies
{
    public readonly struct EnemyDecision
    {
        public EnemyDecision(EnemyState state, Vector3 direction)
        {
            State = state;
            Direction = direction;
        }

        public EnemyState State { get; }
        public Vector3 Direction { get; }
    }
}
