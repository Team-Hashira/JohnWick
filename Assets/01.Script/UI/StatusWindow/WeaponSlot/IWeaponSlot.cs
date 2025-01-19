using Hashira.Items.Weapons;

namespace Hashira.UI.StatusWindow
{
    public interface IWeaponSlot
    {
        public int SlotIndex { get; set; }
        public Weapon BaseWeapon { get; set; }
        public void HandleWeaponChanged(Weapon weapon);
    }
}
