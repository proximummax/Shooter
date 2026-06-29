using System;
using Shooter.Abilities;
using Shooter.Arena;
using Shooter.Enemies;
using Shooter.Projectiles;
using Shooter.Weapons;

namespace Shooter.Composition
{
    public static class RuntimeContentValidator
    {
        public static WeaponDefinition RequireValidWeapon(WeaponDefinition weapon, string owner)
        {
            if (weapon == null)
            {
                throw new InvalidOperationException($"{owner} requires a weapon definition.");
            }

            AbilityLoadoutDefinition abilityLoadout = weapon.AbilityLoadout;
            if (abilityLoadout.AbilitySlots.Count == 0)
            {
                throw new InvalidOperationException($"{owner} weapon '{weapon.Id}' must contain at least one ability slot.");
            }

            for (int i = 0; i < abilityLoadout.AbilitySlots.Count; i++)
            {
                AbilitySlotDefinition slot = abilityLoadout.AbilitySlots[i];
                if (slot == null || slot.Ability == null)
                {
                    throw new InvalidOperationException($"{owner} weapon '{weapon.Id}' has an empty ability slot at index {i}.");
                }

                RequireValidAbility(slot.Ability, owner);
            }

            return weapon;
        }

        public static WaveDefinition RequireValidWave(WaveDefinition wave, string owner)
        {
            if (wave == null)
            {
                throw new InvalidOperationException($"{owner} requires a wave definition.");
            }

            if (wave.Entries.Count == 0)
            {
                throw new InvalidOperationException($"{owner} wave must contain at least one entry.");
            }

            for (int i = 0; i < wave.Entries.Count; i++)
            {
                WaveEntryDefinition entry = wave.Entries[i];
                if (entry == null)
                {
                    throw new InvalidOperationException($"{owner} wave has an empty entry at index {i}.");
                }

                if (entry.Archetype == null)
                {
                    throw new InvalidOperationException($"{owner} wave entry '{entry.NamePrefix}' must reference an enemy archetype.");
                }

                if (entry.Archetype.EnemyPrefab == null)
                {
                    throw new InvalidOperationException($"{owner} enemy archetype '{entry.Archetype.Id}' must reference an enemy prefab.");
                }

                if (entry.Archetype.EnemyPrefab.GetComponent<PooledEnemy>() == null)
                {
                    throw new InvalidOperationException($"{owner} enemy archetype '{entry.Archetype.Id}' prefab must contain {nameof(PooledEnemy)}.");
                }

                if (entry.Archetype.Stats == null)
                {
                    throw new InvalidOperationException($"{owner} enemy archetype '{entry.Archetype.Id}' must reference stats.");
                }
            }

            return wave;
        }

        public static ProjectileDefinition RequireValidProjectile(ProjectileDefinition projectile, string owner)
        {
            if (projectile == null)
            {
                throw new InvalidOperationException($"{owner} projectile ability must reference a projectile definition.");
            }

            if (projectile.ProjectilePrefab == null)
            {
                throw new InvalidOperationException($"{owner} projectile definition '{projectile.Id}' must reference a projectile prefab.");
            }

            return projectile;
        }

        private static void RequireValidAbility(AbilityDefinition ability, string owner)
        {
            if (ability.Effect == null)
            {
                throw new InvalidOperationException($"{owner} ability '{ability.Id}' must reference an ability effect.");
            }

            if (ability.Effect is ProjectileAbilityEffectDefinition projectileEffect)
            {
                RequireValidProjectile(projectileEffect.Projectile, $"{owner} ability '{ability.Id}'");
            }

            if (ability.Effect is DashAbilityEffectDefinition dashEffect && dashEffect.Distance <= 0f)
            {
                throw new InvalidOperationException($"{owner} ability '{ability.Id}' dash distance must be greater than zero.");
            }
        }
    }
}
