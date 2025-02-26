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
            _projectile = projectile;
            _projectile.SetAttackType(EAttackType.HeadShot);
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);
            _projectile.SetAttackType();
        }
    }
}
