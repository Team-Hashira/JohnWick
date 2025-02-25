using Hashira.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class AimingProjectileModifier : ProjectileModifier
    {
        [SerializeField] private float _delay = 10f;
        
        private bool _isCanAimingBullet = false;

        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
            _attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (CooldownUtillity.CheckCooldown("AimingBullet", _delay) && _isCanAimingBullet == false)
            {
                _isCanAimingBullet = true;
                _attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            }
        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
            if (CooldownUtillity.CheckCooldown("AimingBullet", _delay))
                HandleProjectileCreateEvent(null);
        }

        private void HandleProjectileCreateEvent(List<Projectile> projectileList)
        {
            _attacker.OnProjectileCreateEvent -= HandleProjectileCreateEvent;
            CooldownUtillity.StartCooldown("AimingBullet");
            _isCanAimingBullet = false;
        }

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
