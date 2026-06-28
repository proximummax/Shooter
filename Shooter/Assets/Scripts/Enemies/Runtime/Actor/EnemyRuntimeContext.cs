using System;
using Shooter.Arena;
using Shooter.Combat;
using UnityEngine;

namespace Shooter.Enemies
{
    public sealed class EnemyRuntimeContext
    {
        public EnemyRuntimeContext(
            Transform target,
            IGameSessionStateReader sessionState,
            IDamagePipelineProvider damagePipelineProvider,
            Camera worldUiCamera = null)
        {
            Target = target != null ? target : throw new ArgumentNullException(nameof(target));
            SessionState = sessionState != null ? sessionState : throw new ArgumentNullException(nameof(sessionState));
            DamagePipelineProvider = damagePipelineProvider ?? new StandardDamagePipelineProvider();
            WorldUiCamera = worldUiCamera;
        }

        public Transform Target { get; }
        public IGameSessionStateReader SessionState { get; }
        public IDamagePipelineProvider DamagePipelineProvider { get; }
        public Camera WorldUiCamera { get; }
        public DamagePipeline DamagePipeline => DamagePipelineProvider.Combat;
    }
}
