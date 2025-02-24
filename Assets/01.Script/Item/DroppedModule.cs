using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Items.Module;
using Hashira.Players;

namespace Hashira.Items
{
    public class DroppedModule : DroppedItem
    {
        private ModuleItem ModuleItem => _item as ModuleItem;

        public override void Interaction(Player player)
        {
            base.Interaction(player);
            player.GetEntityComponent<PlayerModule>().AddModule(ModuleItem);
            this.Push();
        }
    }
}
