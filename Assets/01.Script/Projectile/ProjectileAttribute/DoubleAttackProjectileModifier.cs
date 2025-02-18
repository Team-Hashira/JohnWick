using Hashira.Entities;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class DoubleAttackProjectileModifier : ProjectileModifier
    {
        public int doubleAttackDamage = 100;

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);
            if (damageable != null)
            {
                damageable.ApplyDamage(doubleAttackDamage, hit, _projectile.transform, attackType: EAttackType.Fixed);
            }
        }
    }
}
