using Hashira.Players;
using Hashira.Projectiles;
using System;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class AimingBulletModule : Module
    {
        private HeadShotProjectileModifier _headShotProjectile;

        private float _delay = 10f;

        public override void Equip(Player player)
        {
            base.Equip(player);
            _headShotProjectile = new HeadShotProjectileModifier();
            _headShotProjectile.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            _player.Attacker.AddProjectileModifiers(_headShotProjectile);
        }

        private void HandleProjectileCreateEvent()
        {
            _headShotProjectile.OnProjectileCreateEvent -= HandleProjectileCreateEvent;
            _player.Attacker.RemoveProjectileModifiers(_headShotProjectile);
            CooldownUtillity.StartCooldown("AimingBullet");
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();

            if (CooldownUtillity.CheckCooldown("AimingBullet", _delay))
            {
                Debug.Log("사용 가능");
                _headShotProjectile.OnProjectileCreateEvent += HandleProjectileCreateEvent;
                _player.Attacker.AddProjectileModifiers(_headShotProjectile);
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();
            if (CooldownUtillity.CheckCooldown("AimingBullet", _delay))
                HandleProjectileCreateEvent();
        }
    }
}
 