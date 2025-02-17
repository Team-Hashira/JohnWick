using Hashira.Entities;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class DoubleAttackProjectileModifier : ProjectileModifier
    {
        public int doubleAttackDamage = 100;

        public override void OnHitedDamageable(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnHitedDamageable(hit, damageable);
            EEntityPartType parts = damageable.ApplyDamage(doubleAttackDamage, hit, _projectile.transform);
        }
    }
}
