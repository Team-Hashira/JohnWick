using Crogen.CrogenPooling;
using Hashira.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class BombProjectileModifier : ProjectileModifier
    {
        [SerializeField] private int _damage = 150;

        private ProjectilePoolType _prevProjectilePoolType;


        public override void OnProjectileCreate(Projectile projectile)
        {
            _prevProjectilePoolType = _attacker.SetProjectile(ProjectilePoolType.Grenade);

            base.OnProjectileCreate(projectile);
            _attacker.SetProjectile(_prevProjectilePoolType);
            projectile.SetAttackType(EAttackType.Fire);
            projectile.SetDamage(_damage);
            //ModifierExecuter.Reset();
        }

        public override void OnProjectileHit(Projectile projectile, HitInfo hitInfo)
        {
            base.OnProjectileHit(projectile, hitInfo);

            projectile.SetAttackType();
            projectile.gameObject.Pop(EffectPoolType.BoomFire, hitInfo.raycastHit.point, Quaternion.identity);
        }
    }
}
