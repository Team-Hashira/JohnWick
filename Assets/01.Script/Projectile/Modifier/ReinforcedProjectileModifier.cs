using UnityEngine;

namespace Hashira.Projectiles
{
    public class ReinforcedProjectileModifier : ProjectileModifier
    {
        [field: SerializeField]
        public int AddValue { get; private set; } = 0;
        [field: SerializeField]
        public float MultiplyValue { get; private set; } = 1f;

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            projectile.DamageOverride((int)(projectile.Damage * MultiplyValue) + AddValue);
        }
    }
}
