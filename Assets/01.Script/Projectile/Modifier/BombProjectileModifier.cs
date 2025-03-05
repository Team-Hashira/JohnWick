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

        private bool _isFirst;

        public override void OnProjectileCreateReady()
        {
            base.OnProjectileCreateReady();
            _prevProjectilePoolType = _attacker.SetProjectile(ProjectilePoolType.Grenade);
            _isFirst = true;
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            if (_isFirst)
            {
                _attacker.SetProjectile(_prevProjectilePoolType);
                _projectile.SetAttackType(EAttackType.Fire);
                _projectile.DamageOverride(_damage);
                _isFirst = false;
                //ModifierExecuter.Reset();
            }
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);

            _projectile.SetAttackType();
            _projectile.gameObject.Pop(EffectPoolType.BoomFire, hit.point, Quaternion.identity);
        }
    }
}
