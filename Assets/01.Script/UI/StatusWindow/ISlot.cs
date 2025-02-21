using Hashira.Core.StatSystem;
using Hashira.Items;

namespace Hashira.UI.StatusWindow
{
    public interface ISlot
    {
        public Item Item { get; set; }

        public IStatable GetStatable();
    }
}
