using UnityEngine;

namespace Shooter.Combat
{
    public readonly struct AreaDamageIntent
    {
        public AreaDamageIntent(DamageIntent damage, Vector3 center, float radius, int layerMask)
        {
            Damage = damage;
            Center = center;
            Radius = Mathf.Max(0f, radius);
            LayerMask = layerMask;
        }

        public DamageIntent Damage { get; }
        public Vector3 Center { get; }
        public float Radius { get; }
        public int LayerMask { get; }
    }
}
