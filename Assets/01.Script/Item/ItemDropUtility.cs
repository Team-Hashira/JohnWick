using Crogen.CrogenPooling;
using Hashira.Entities.Interacts;
using UnityEngine;

namespace Hashira.Items
{
    public static class ItemDropUtility
    {
        private static GameObject _gameObject;

        private static void Init()
        {
            _gameObject ??= GameManager.Instance.gameObject;
        }
        
        public static DroppedWeapon DropWeaponItem(Vector2 position)
        {
            Init();
            
            var weaponItem = _gameObject.Pop(ItemPoolType.WeaponItem, position, Quaternion.identity) as DroppedWeapon;
            return null;
        }

        public static DroppedParts DropPartsItem()
        {
            Init();
            return null;
        }
    }
}
