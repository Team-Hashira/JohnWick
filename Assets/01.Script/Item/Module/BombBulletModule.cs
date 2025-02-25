using Hashira.Players;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class BombBulletModule : Module
    {
        private BombProjectileModifier _bombProjectileModifier;

        private float _delay = 2f;
        private bool _isCanAimingBullet = false;

        public override void Equip(Player player)
        {
            base.Equip(player);
            _bombProjectileModifier = new BombProjectileModifier();
            _player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            _player.Attacker.AddProjectileModifiers(_bombProjectileModifier);
        }

        private void HandleProjectileCreateEvent()
        {
            _player.Attacker.OnProjectileCreateEvent -= HandleProjectileCreateEvent;
            _player.Attacker.RemoveProjectileModifiers(_bombProjectileModifier);
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
                _player.Attacker.AddProjectileModifiers(_bombProjectileModifier);
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();
            if (CooldownUtillity.CheckCooldown("BombBullet", _delay))
                HandleProjectileCreateEvent();
        }
    }
}
