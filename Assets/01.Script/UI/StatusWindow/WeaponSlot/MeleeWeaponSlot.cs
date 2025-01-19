using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class MeleeWeaponSlot : MonoBehaviour, IWeaponSlot
    {
        [SerializeField] private WeaponSlotIcon _icon;

        public int SlotIndex { get; set; }
        public Weapon BaseWeapon { get; set; }

        public void HandleWeaponChanged(Weapon weapon)
        {
            Debug.Log(weapon.WeaponSO.itemName);
            BaseWeapon = weapon;
            _icon.Init(this);
        }
    }
}
