using System;
using System.Collections.Generic;

namespace Shooter.Abilities
{
    public sealed class AbilityExecutorRegistry
    {
        private readonly Dictionary<AbilityType, IAbilityExecutor> _executors = new Dictionary<AbilityType, IAbilityExecutor>();

        public AbilityExecutorRegistry(IEnumerable<IAbilityExecutor> executors)
        {
            if (executors == null)
            {
                return;
            }

            foreach (IAbilityExecutor executor in executors)
            {
                if (executor == null)
                {
                    continue;
                }

                _executors[executor.AbilityType] = executor;
            }
        }

        public void Execute(AbilityDefinition ability, AbilityUseContext context, int damageLayerMask)
        {
            if (ability == null)
            {
                throw new ArgumentNullException(nameof(ability));
            }

            if (!_executors.TryGetValue(ability.Type, out IAbilityExecutor executor))
            {
                throw new InvalidOperationException($"Unsupported ability type '{ability.Type}'.");
            }

            executor.Execute(ability, context, damageLayerMask);
        }
    }
}
