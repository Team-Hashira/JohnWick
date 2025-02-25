using Hashira.Players;
using Hashira.Projectiles;

namespace Hashira.Items.Modules
{
    public class BlastBulletModule : Module
    {
        private BlastBulletModifier _modifier;
        private bool _isCanBlastBullet = true;
        float _delay = 8f
            ;
        public override void Equip(Player player)
        {
            base.Equip(player);
            _modifier = new BlastBulletModifier();
            player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            player.Attacker.AddProjectileModifiers(_modifier);
        }

        private void HandleProjectileCreateEvent()
        {
            _player.Attacker.OnProjectileCreateEvent -= HandleProjectileCreateEvent;
            _player.Attacker.RemoveProjectileModifiers(_modifier);
            CooldownUtillity.StartCooldown("BlastBullet");
            _isCanBlastBullet = false;
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();
            if (CooldownUtillity.CheckCooldown("BlastBullet", _delay) && _isCanBlastBullet == false)
            {
                _isCanBlastBullet = true;
                _player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
                _player.Attacker.AddProjectileModifiers(_modifier);
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
