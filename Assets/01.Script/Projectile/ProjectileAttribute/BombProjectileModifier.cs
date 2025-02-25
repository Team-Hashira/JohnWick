using Crogen.CrogenPooling;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class BombProjectileModifier : ProjectileModifier
    {
        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            _projectile.SetAttackType(EAttackType.Fire);
            _projectile.DamageOverride(150);
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);
            _projectile.SetAttackType();
            _projectile.gameObject.Pop(EffectPoolType.BoomFire, hit.point, Quaternion.identity);
        }
    }
}
