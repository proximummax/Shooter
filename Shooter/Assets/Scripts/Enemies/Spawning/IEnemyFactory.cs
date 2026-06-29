using UnityEngine;

namespace Shooter.Enemies
{
    public interface IEnemyFactory
    {
        PooledEnemy Create(EnemyArchetypeDefinition archetype, Vector3 position, string name);
    }
}
