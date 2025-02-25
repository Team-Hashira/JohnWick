using Hashira.Entities;
using Hashira.Players;
using Hashira.Projectiles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class AimingBulletModule : Module
    {
        private Projectile _projectile;

        private float _delay = 10f;
        private bool _isCanAimingBullet = false;

        public override void Equip(Player player)
        {
            base.Equip(player);
            _player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            //_player.Attacker.AddProjectileModifiers(this);
        }

        private void HandleProjectileCreateEvent(List<Projectile> projectileList)
        {
            _player.Attacker.OnProjectileCreateEvent -= HandleProjectileCreateEvent;
            //_player.Attacker.RemoveProjectileModifiers(this);
            CooldownUtillity.StartCooldown("AimingBullet");
            _isCanAimingBullet = false;
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();

            if (CooldownUtillity.CheckCooldown("AimingBullet", _delay) && _isCanAimingBullet == false)
            {
                _isCanAimingBullet = true;
                _player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
                //_player.Attacker.AddProjectileModifiers(this);
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();
            if (CooldownUtillity.CheckCooldown("AimingBullet", _delay))
                HandleProjectileCreateEvent(null);
        }

        public void OnProjectileCreate(Projectile projectile)
        {
            _projectile = projectile;
            _projectile.SetAttackType(EAttackType.HeadShot);
        }

        public void OnProjectileUpdate()
        {

        }

        public void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            _projectile.SetAttackType();
        }
    }
}
 