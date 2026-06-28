using System;
using System.Collections.Generic;
using Shooter.Combat;
using Shooter.Projectiles;

namespace Shooter.Abilities
{
    public sealed class AbilityLoadout
    {
        private readonly List<AbilityRuntimeSlot> _slots = new List<AbilityRuntimeSlot>();
        private readonly Dictionary<AbilitySlotId, AbilityRuntimeSlot> _slotsById = new Dictionary<AbilitySlotId, AbilityRuntimeSlot>();
        private AbilityLoadoutDefinition _definition;
        private int _damageLayerMask = ~0;
        private AbilityExecutorRegistry _executorRegistry;
        private bool _isInitialized;

        public event Action StateChanged;

        public AbilityLoadoutDefinition CurrentDefinition => _definition;
        public IReadOnlyList<AbilityRuntimeSlot> Slots => _slots;

        public void Initialize(AbilityLoadoutDefinition definition, int damageLayerMask)
        {
            _definition = definition;
            _damageLayerMask = damageLayerMask;
            EnsureInitialized(force: true);
        }

        public void ConfigureExecutors(IProjectileFactory projectileFactory, IDamagePipelineProvider damagePipelineProvider)
        {
            _executorRegistry = CreateExecutorRegistry(projectileFactory, damagePipelineProvider);
            EnsureInitialized(force: true);
        }

        public AbilityRuntimeSlot GetSlot(AbilitySlotId slotId)
        {
            EnsureInitialized();
            return _slotsById.TryGetValue(slotId, out AbilityRuntimeSlot slot) ? slot : null;
        }

        public AbilityDefinition GetAbility(AbilitySlotId slotId)
        {
            return GetSlot(slotId)?.Ability;
        }

        public AbilityCooldown GetCooldown(AbilitySlotId slotId)
        {
            return GetSlot(slotId)?.Cooldown;
        }

        public bool TryUse(AbilitySlotId slotId, AbilityUseContext context)
        {
            EnsureInitialized();
            if (!_isInitialized || !_slotsById.TryGetValue(slotId, out AbilityRuntimeSlot slot) || !slot.Cooldown.CanUse(context.Time))
            {
                return false;
            }

            (_executorRegistry ??= CreateExecutorRegistry(null, new StandardDamagePipelineProvider())).Execute(
                slot.Ability,
                context,
                _damageLayerMask);
            slot.Cooldown.Start(context.Time);
            StateChanged?.Invoke();
            return true;
        }

        private void EnsureInitialized(bool force = false)
        {
            if (_isInitialized && !force)
            {
                return;
            }

            if (force)
            {
                _isInitialized = false;
            }

            _slots.Clear();
            _slotsById.Clear();

            if (_definition == null)
            {
                return;
            }

            foreach (AbilitySlotDefinition slotDefinition in _definition.AbilitySlots)
            {
                if (slotDefinition == null || !slotDefinition.HasAbility)
                {
                    continue;
                }

                var runtimeSlot = new AbilityRuntimeSlot(slotDefinition);
                _slots.Add(runtimeSlot);
                _slotsById[runtimeSlot.SlotId] = runtimeSlot;
            }

            _isInitialized = _slots.Count > 0;
            StateChanged?.Invoke();
        }

        private static AbilityExecutorRegistry CreateExecutorRegistry(
            IProjectileFactory projectileFactory,
            IDamagePipelineProvider damagePipelineProvider)
        {
            return new AbilityExecutorRegistry(new IAbilityExecutor[]
            {
                new ProjectileAbilityExecutor(projectileFactory),
                new AreaDamageAbilityExecutor(damagePipelineProvider),
                new DashAbilityExecutor()
            });
        }
    }
}
