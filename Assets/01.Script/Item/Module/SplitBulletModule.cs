using Hashira.Players;
using UnityEngine;

namespace Hashira.Items.Module
{
    public class SplitBulletModule : Module
    {
        public override void Equip(Player player)
        {
            base.Equip(player);
            _player.bulletCount++;
        }

        public override void UnEquip()
        {
            base.UnEquip();
            _player.bulletCount--;
        }
    }
}
