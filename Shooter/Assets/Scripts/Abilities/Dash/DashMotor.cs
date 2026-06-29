using UnityEngine;

namespace Shooter.Abilities
{
    public sealed class DashMotor : MonoBehaviour
    {
        private CharacterController _characterController;
        private Vector3 _direction;
        private float _remainingDistance;
        private float _speed;

        public bool IsDashing => _remainingDistance > 0f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void StartDash(Vector3 direction, float distance, float duration)
        {
            if (direction.sqrMagnitude <= 0.001f || distance <= 0f)
            {
                _remainingDistance = 0f;
                _speed = 0f;
                return;
            }

            _direction = direction.normalized;
            _remainingDistance = Mathf.Max(0f, distance);
            _speed = _remainingDistance / Mathf.Max(0.01f, duration);
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        public void Tick(float deltaTime)
        {
            if (!IsDashing || deltaTime <= 0f)
            {
                return;
            }

            float step = Mathf.Min(_remainingDistance, _speed * deltaTime);
            Vector3 displacement = _direction * step;
            if (_characterController != null && _characterController.enabled)
            {
                _characterController.Move(displacement);
            }
            else
            {
                transform.position += displacement;
            }

            _remainingDistance -= step;
            if (_remainingDistance <= 0.0001f)
            {
                _remainingDistance = 0f;
            }
        }
    }
}
