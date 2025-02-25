using Hashira.Players;
using Hashira.Projectiles;

namespace Hashira.Items.Modules
{
    public class BlastBulletModule : Module
    {
        private BlastBulletModifier _modifier;

        public override void Equip(Player player)
        {
            base.Equip(player);
            _modifier = new BlastBulletModifier();
            player.Attacker.AddProjectileModifiers(_modifier);
        }

        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();
        }

        public override void UnEquip()
        {
            base.UnEquip();
            _player.Attacker.RemoveProjectileModifiers(_modifier);
        }
    }
}
