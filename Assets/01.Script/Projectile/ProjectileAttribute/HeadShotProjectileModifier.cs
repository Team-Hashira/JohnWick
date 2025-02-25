using Hashira.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class HeadShotProjectileModifier : ProjectileModifier
    {
        public event Action OnProjectileCreateEvent;
        
        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            _projectile.SetAttackType(EAttackType.HeadShot);
            OnProjectileCreateEvent?.Invoke();
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);
            _projectile.SetAttackType();
        }
    }
}
