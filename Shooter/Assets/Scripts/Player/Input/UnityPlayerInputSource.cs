using UnityEngine;

namespace Shooter.Player
{
    public sealed class UnityPlayerInputSource : MonoBehaviour, IPlayerInputSource
    {
        private Camera _camera;
        private Vector3 _lastAimDirection = Vector3.forward;

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        public void Initialize(Camera gameplayCamera)
        {
            _camera = gameplayCamera != null ? gameplayCamera : Camera.main;
        }

        public PlayerInputState Read()
        {
            Vector3 move = new Vector3(
                Input.GetAxisRaw("Horizontal"),
                0f,
                Input.GetAxisRaw("Vertical"));

            UpdateAimDirection();

            return new PlayerInputState(
                move,
                _lastAimDirection,
                Input.GetMouseButtonDown(0),
                Input.GetKeyDown(KeyCode.Q),
                Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift));
        }

        private void UpdateAimDirection()
        {
            if (_camera == null)
            {
                return;
            }

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            var ground = new Plane(Vector3.up, Vector3.zero);
            if (!ground.Raycast(ray, out float distance))
            {
                return;
            }

            Vector3 point = ray.GetPoint(distance);
            Vector3 direction = point - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.001f)
            {
                _lastAimDirection = direction.normalized;
            }
        }
    }
}
