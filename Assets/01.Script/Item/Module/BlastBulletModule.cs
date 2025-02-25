using Crogen.CrogenPooling;
using Hashira.Players;
using Hashira.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class BlastBulletModule : Module
    {
        float _delay = 8;
        private int _damage = 100;
        public Projectile _projectile;

        public override void Equip(Player player)
        {
            base.Equip(player);
            player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            //player.Attacker.AddProjectileModifiers(this);
        }

        private void HandleProjectileCreateEvent(List<Projectile> projectileList)
        {
            _player.Attacker.OnProjectileCreateEvent -= HandleProjectileCreateEvent;
            //_player.Attacker.RemoveProjectileModifiers(this);
            CooldownUtillity.StartCooldown("BlastBullet");
            //_isCanBlastBullet = false;
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();
            //if (CooldownUtillity.CheckCooldown("BlastBullet", _delay) && _isCanBlastBullet == false)
            //{
            //    _isCanBlastBullet = true;
            //    _player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            //    //_player.Attacker.AddProjectileModifiers(this);
            //}
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
        }

        public void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            if (CooldownUtillity.CheckCooldown("BlastBullet", _delay, true) == false) return;
            _damage = (int)(_projectile.Damage * 0.3f);

            for (int i = -1; i <= 1; i++)
            {
                var blastBulletModuleSplinter = PopCore.Pop(ProjectilePoolType.BlastBulletModuleSplinter, _projectile.transform.position + Vector3.up * 0.25f, Quaternion.Euler(0, 0, Random.Range(0, 360f))) as BlastBulletModuleSplinter;
                Vector2 dir = new Vector2(i * 0.55f, 1).normalized * 300;
                dir += Random.insideUnitCircle.normalized * 10f;
                blastBulletModuleSplinter.Init(dir, _damage);
            }
            CooldownUtillity.StartCooldown("BlastBullet");
        }

        public void OnProjectileUpdate()
        {
        }
    }
}
