using Crogen.CrogenPooling;
using Hashira.Entities.Interacts;
using Hashira.Items.WeaponPartsSystem;
using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.Items
{
    public class ItemDropUtility : MonoBehaviour
    {
        private static GameObject _gameObject;
        
        public static DroppedWeapon DropWeapon(WeaponSO weapon, Vector2 position)
        {
            var weaponItem = _gameObject.Pop(ItemPoolType.WeaponItem, position, Quaternion.identity) as DroppedWeapon;
            if (weaponItem != null) weaponItem.SetWeapon(weapon.GetWeaponClass());
            return weaponItem;
        }

        public static DroppedParts DropParts(WeaponPartsSO parts, Vector2 position)
        {
            var partsItem = _gameObject.Pop(ItemPoolType.WeaponPartsItem, position, Quaternion.identity) as DroppedParts;
            if (partsItem != null) partsItem.SetParts(parts.GetWeaponPartsClass());
            return partsItem;
        }
    }
}
