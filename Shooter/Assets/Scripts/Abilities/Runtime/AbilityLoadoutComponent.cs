using System;
using System.Collections.Generic;
using Shooter.Combat;
using Shooter.Projectiles;
using UnityEngine;
using VContainer;

namespace Shooter.Abilities
{
    public sealed class AbilityLoadoutComponent : MonoBehaviour
    {
        private readonly AbilityLoadout _loadout = new AbilityLoadout();
        private bool _eventsBound;

        public event Action AbilityStateChanged;

        public AbilityLoadoutDefinition CurrentDefinition => _loadout.CurrentDefinition;
        public IReadOnlyList<AbilityRuntimeSlot> Slots => _loadout.Slots;

        private void Awake()
        {
            BindLoadoutEvents();
        }

        public void Initialize(AbilityLoadoutDefinition definition, int damageLayerMask)
        {
            BindLoadoutEvents();
            _loadout.Initialize(definition, damageLayerMask);
        }

        [Inject]
        public void Construct(IProjectileFactory projectileFactory, IDamagePipelineProvider damagePipelineProvider)
        {
            BindLoadoutEvents();
            _loadout.ConfigureExecutors(projectileFactory, damagePipelineProvider);
        }

        private void BindLoadoutEvents()
        {
            if (_eventsBound)
            {
                return;
            }

            _loadout.StateChanged += HandleLoadoutStateChanged;
            _eventsBound = true;
        }

        public AbilityRuntimeSlot GetSlot(AbilitySlotId slotId)
        {
            return _loadout.GetSlot(slotId);
        }

        public AbilityDefinition GetAbility(AbilitySlotId slotId)
        {
            return _loadout.GetAbility(slotId);
        }

        public AbilityCooldown GetCooldown(AbilitySlotId slotId)
        {
            return _loadout.GetCooldown(slotId);
        }

        public bool TryUse(AbilitySlotId slotId, Vector3 aimDirection, float now)
        {
            return TryUse(slotId, aimDirection, Vector3.zero, now);
        }

        public bool TryUse(AbilitySlotId slotId, Vector3 aimDirection, Vector3 moveDirection, float now)
        {
            return _loadout.TryUse(slotId, new AbilityUseContext(transform, aimDirection, moveDirection, now));
        }

        private void HandleLoadoutStateChanged()
        {
            AbilityStateChanged?.Invoke();
        }
    }
}
