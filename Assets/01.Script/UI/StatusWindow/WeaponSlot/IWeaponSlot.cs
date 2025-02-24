using Hashira.Items.SubItems;
using Hashira.Items.Weapons;

namespace Hashira.UI.StatusWindow
{
    public interface IWeaponSlot : ISlot
    {
        public int SlotIndex { get; set; }
        public void HandleWeaponChanged(Weapon weapon);
    }
    public interface ISubItemSlot : ISlot
    {
        public int SlotIndex { get; set; }
        public void HandleSubItemChanged(SubItem weapon);
    }
}
