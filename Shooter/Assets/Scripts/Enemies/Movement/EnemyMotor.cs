using UnityEngine;

namespace Shooter.Enemies
{
    public sealed class EnemyMotor
    {
        private readonly Transform _transform;

        public EnemyMotor(Transform transform)
        {
            _transform = transform;
        }

        public void Move(Vector3 direction, float speed, float deltaTime)
        {
            if (direction.sqrMagnitude <= 0.001f || speed <= 0f || deltaTime <= 0f)
            {
                return;
            }

            Vector3 normalized = direction.normalized;
            _transform.position += normalized * (speed * deltaTime);
            Face(normalized);
        }

        public void Face(Vector3 direction)
        {
            if (direction.sqrMagnitude <= 0.001f)
            {
                return;
            }

            _transform.rotation = Quaternion.LookRotation(direction.normalized);
        }
    }
}
