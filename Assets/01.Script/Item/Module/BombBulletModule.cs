using Crogen.CrogenPooling;
using Hashira.Entities;
using Hashira.Players;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class BombBulletModule : Module, IProjectileModifier
    {
        private Projectile _projectile;

        private float _delay = 2f;
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
                _player.Attacker.AddProjectileModifiers(this);
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();
            if (CooldownUtillity.CheckCooldown("BombBullet", _delay))
                HandleProjectileCreateEvent();
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
