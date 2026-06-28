using UnityEngine;

namespace Shooter.Effects
{
    public interface IHitParticleFactory
    {
        HitParticleEffect Play(GameObject prefab, Vector3 position, Vector3 normal);
    }
}
