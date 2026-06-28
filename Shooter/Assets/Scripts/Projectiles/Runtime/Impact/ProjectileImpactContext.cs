using UnityEngine;

namespace Shooter.Projectiles
{
    public readonly struct ProjectileImpactContext
    {
        public ProjectileImpactContext(Collider collider, Vector3 position, Vector3 normal)
        {
            Collider = collider;
            Position = position;
            Normal = normal;
        }

        public Collider Collider { get; }
        public Vector3 Position { get; }
        public Vector3 Normal { get; }
    }
}
