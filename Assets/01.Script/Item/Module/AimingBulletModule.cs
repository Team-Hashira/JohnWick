using Hashira.Entities;
using Hashira.Players;
using Hashira.Projectiles;
using System;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class AimingBulletModule : Module, IProjectileModifier
    {
        private Projectile _projectile;

        private float _delay = 10f;
        private bool _isCanAimingBullet = false;

        public override void Equip(Player player)
        {
            base.Equip(player);
            _player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            _player.Attacker.AddProjectileModifiers(this);
        }

        private void HandleProjectileCreateEvent()
        {
            _player.Attacker.OnProjectileCreateEvent -= HandleProjectileCreateEvent;
            _player.Attacker.RemoveProjectileModifiers(this);
            CooldownUtillity.StartCooldown("AimingBullet");
            _isCanAimingBullet = false;
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();

            if (CooldownUtillity.CheckCooldown("AimingBullet", _delay) && _isCanAimingBullet == false)
            {
                Debug.Log("사용 가능");
                _isCanAimingBullet = true;
                _player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
                _player.Attacker.AddProjectileModifiers(this);
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();
            if (CooldownUtillity.CheckCooldown("AimingBullet", _delay))
                HandleProjectileCreateEvent();
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
 