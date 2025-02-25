using Crogen.CrogenPooling;
using Hashira.Entities;
using Hashira.Players;
using Hashira.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class BombBulletModule : Module, IProjectileModifier
    {
        private Projectile _projectile;

        private float _delay = 12f;
        private bool _isCanAimingBullet = false;
        private ProjectilePoolType _prevProjectilePoolType;

        public override void Equip(Player player)
        {
            base.Equip(player);
            _player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            _prevProjectilePoolType = _player.Attacker.SetProjectile(ProjectilePoolType.Grenade);
            //_player.Attacker.AddProjectileModifiers(this);
        }

        private void HandleProjectileCreateEvent(List<Projectile> projectileList)
        {
            _player.Attacker.OnProjectileCreateEvent -= HandleProjectileCreateEvent;
            _player.Attacker.SetProjectile(_prevProjectilePoolType);
            //_player.Attacker.RemoveProjectileModifiers(this);
            CooldownUtillity.StartCooldown("BombBullet");
            _isCanAimingBullet = false;
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();

            if (CooldownUtillity.CheckCooldown("BombBullet", _delay) && _isCanAimingBullet == false)
            {
                _isCanAimingBullet = true;
                _player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
                _prevProjectilePoolType = _player.Attacker.SetProjectile(ProjectilePoolType.Grenade);
                //_player.Attacker.AddProjectileModifiers(this);
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();
            if (CooldownUtillity.CheckCooldown("BombBullet", _delay))
                HandleProjectileCreateEvent(null);
        }


        public void OnProjectileCreate(Projectile projectile)
        {
            _projectile = projectile;
            _projectile.SetAttackType(EAttackType.Fire);
            _projectile.DamageOverride(150);
        }

        public void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            _projectile.SetAttackType();
            _projectile.gameObject.Pop(EffectPoolType.BoomFire, hit.point, Quaternion.identity);
        }

        public void OnProjectileUpdate()
        {
        }
    }
}
