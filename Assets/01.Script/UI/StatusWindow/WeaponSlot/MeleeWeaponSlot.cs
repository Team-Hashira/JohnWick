using Hashira.Items.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class MeleeWeaponSlot : MonoBehaviour, IWeaponSlot
    {
        [SerializeField] private Image _iconImage;
        public Weapon baseWeapon;

        public int SlotIndex { get; set; }

        public void HandleWeaponChanged(Weapon weapon)
        {
            Debug.Log(weapon.WeaponSO.itemName);
            if (weapon is not MeleeWeapon) return;
            
            baseWeapon = weapon;
            _iconImage.sprite = weapon.WeaponSO.itemSprite;
        }
    }
}
