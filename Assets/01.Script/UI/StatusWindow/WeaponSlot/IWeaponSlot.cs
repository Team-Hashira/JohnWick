using Hashira.Items.Weapons;

namespace Hashira.UI.StatusWindow
{
    public interface IWeaponSlot
    {
        public void HandleWeaponChanged(Weapon weapon);
    }
}
