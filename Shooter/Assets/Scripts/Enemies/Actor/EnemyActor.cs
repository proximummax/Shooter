using Shooter.Arena;
using Shooter.Characters;
using Shooter.Combat;
using UnityEngine;
using VContainer.Unity;

namespace Shooter.Enemies
{
    [RequireComponent(typeof(HealthComponent))]
    public sealed class EnemyActor : MonoBehaviour
    {
        private Transform _target;
        private CharacterStatsDefinition _stats;
        private HealthComponent _health;
        private IGameSessionStateReader _sessionState;
        private EnemyBehavior _behavior;
        private EnemyMotor _motor;
        private EnemyAttackExecutor _attackExecutor;
        private EnemyPresenter _presenter;

        public EnemyState State => _behavior != null ? _behavior.State : EnemyState.Idle;

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
            _health.Died += HandleDied;
            _behavior = new EnemyBehavior();
            _motor = new EnemyMotor(transform);
            _attackExecutor = new EnemyAttackExecutor();
            _presenter = new EnemyPresenter(gameObject);
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.Died -= HandleDied;
            }
        }

        public void Initialize(
            Transform target,
            CharacterStatsDefinition stats,
            IGameSessionStateReader sessionState,
            ICombatEffectService combatEffects)
        {
            _target = target;
            _stats = stats;
            _sessionState = sessionState;
            _behavior.Reset();
            _attackExecutor = new EnemyAttackExecutor(combatEffects);
            _attackExecutor.Reset();
            _presenter.Restore();
            enabled = true;
        }

        private void Update()
        {
            if (_health == null || _health.IsDead)
            {
                _behavior.MarkDead();
                return;
            }

            if (_sessionState == null || _sessionState.CurrentState != GameSessionState.Running)
            {
                _behavior.SetIdle();
                return;
            }

            if (_target == null || _stats == null)
            {
                _behavior.SetIdle();
                return;
            }

            EnemyDecision decision = _behavior.Evaluate(transform.position, _target.position, _stats.AttackRange);
            if (decision.State == EnemyState.Attack)
            {
                _motor.Face(decision.Direction);
                _attackExecutor.TryAttack(_target, _stats, gameObject, Time.time);
            }
            else if (decision.State == EnemyState.Chase)
            {
                _motor.Move(decision.Direction, _stats.MoveSpeed, Time.deltaTime);
            }
        }

        private void HandleDied(HealthComponent health)
        {
            _behavior.MarkDead();
            _presenter.ShowDead();
            enabled = false;
        }
    }
}
