using Hashira.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class AimingProjectileModifier : ProjectileModifier
    {
        private bool _isCanAimingBullet = false;

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            projectile.SetAttackType(EAttackType.HeadShot);
        }

        public override void OnProjectileHit(Projectile projectile, HitInfo hitInfo)
        {
            base.OnProjectileHit(projectile, hitInfo);
            projectile.SetAttackType();
        }
    }
}
