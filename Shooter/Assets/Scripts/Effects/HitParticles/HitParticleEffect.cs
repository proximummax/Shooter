using System;
using UnityEngine;

namespace Shooter.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class HitParticleEffect : MonoBehaviour
    {
        private ParticleSystem _particles;
        private Action<HitParticleEffect> _release;
        private bool _isPlaying;

        private void Awake()
        {
            _particles = GetComponent<ParticleSystem>();
        }

        public void Initialize(Action<HitParticleEffect> release)
        {
            _release = release;
            if (_particles == null)
            {
                _particles = GetComponent<ParticleSystem>();
            }
        }

        public void Play(Vector3 position, Vector3 normal)
        {
            transform.position = position;
            transform.rotation = normal.sqrMagnitude <= 0.001f
                ? Quaternion.identity
                : Quaternion.LookRotation(normal.normalized);

            if (_particles == null)
            {
                _particles = GetComponent<ParticleSystem>();
            }

            gameObject.SetActive(true);
            _particles.Clear(true);
            _particles.Play(true);
            _isPlaying = true;
        }

        private void Update()
        {
            if (_isPlaying && _particles != null && !_particles.IsAlive(true))
            {
                Release();
            }
        }

        public void Release()
        {
            if (!_isPlaying && !gameObject.activeSelf)
            {
                return;
            }

            if (_particles != null)
            {
                _particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            _isPlaying = false;
            _release?.Invoke(this);
        }
    }
}
