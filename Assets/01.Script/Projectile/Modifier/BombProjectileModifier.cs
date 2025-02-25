using Crogen.CrogenPooling;
using Hashira.Entities;
using Hashira.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class BombProjectileModifier : ProjectileModifier
    {
        [SerializeField] private float _cooldown = 12f;
        private bool _isCanAimingBullet = false;
        private ProjectilePoolType _prevProjectilePoolType;

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            _projectile = projectile;
            _projectile.SetAttackType(EAttackType.Fire);
            _projectile.DamageOverride(150);
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);
            _projectile.SetAttackType();
            _projectile.gameObject.Pop(EffectPoolType.BoomFire, hit.point, Quaternion.identity);
        }

        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
            _attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            _prevProjectilePoolType = _attacker.SetProjectile(ProjectilePoolType.Grenade);
        }

        private void HandleProjectileCreateEvent(List<Projectile> projectileList)
        {
            _attacker.OnProjectileCreateEvent -= HandleProjectileCreateEvent;
            _attacker.SetProjectile(_prevProjectilePoolType);
            CooldownUtillity.StartCooldown("BombBullet");
            _isCanAimingBullet = false;
        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
            if (CooldownUtillity.CheckCooldown("BombBullet", _cooldown))
                HandleProjectileCreateEvent(null);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (CooldownUtillity.CheckCooldown("BombBullet", _cooldown) && _isCanAimingBullet == false)
            {
                _isCanAimingBullet = true;
                _attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
                _prevProjectilePoolType = _attacker.SetProjectile(ProjectilePoolType.Grenade);
            }
        }
    }
}
