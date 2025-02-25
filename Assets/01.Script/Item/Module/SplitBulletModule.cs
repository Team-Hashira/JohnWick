using Hashira.Players;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class SplitBulletModule : Module
    {
        public override void Equip(Player player)
        {
            base.Equip(player);
            _player.Attacker.AddBurstBullets();
        }

        public override void UnEquip()
        {
            base.UnEquip();
            _player.Attacker.RemoveBurstBullets();
        }
    }
}
