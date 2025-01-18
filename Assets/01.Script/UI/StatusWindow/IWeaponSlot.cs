using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public interface IWeaponSlot
    {
        public void HandleWeaponChanged(Weapon weapon);
    }
}
