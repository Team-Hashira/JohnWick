using Hashira.Players;
using Crogen.CrogenPooling;
using UnityEngine;
using System;

namespace Hashira.Items.Modules
{
    public class IceBallModule : Module
    {
        public override void Equip(Player player)
        {
            base.Equip(player);

            //player.Attacker.OnProjectileCreateEvent += HandleOnProjectileCreateEvent;
            CooldownUtillity.StartCooldown("IceBall");
        }

        private void HandleOnProjectileCreateEvent()
        {

        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();
            if (CooldownUtillity.CheckCooldown("IceBall", 12f))
            {
                _player.Attacker.SetProjectile(ProjectilePoolType.IceBall);
            }
        }
    }
}
