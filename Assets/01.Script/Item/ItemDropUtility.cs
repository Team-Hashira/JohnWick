using Crogen.CrogenPooling;
using Hashira.Entities.Interacts;
using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.Items
{
    public class ItemDropUtility : MonoBehaviour
    {
        public static DroppedItem DroppedItem<T>(T item, Vector2 position) where T : Item
        {
            var partsItem = PopCore.Pop(item is Weapon ? ItemPoolType.WeaponItem : ItemPoolType.WeaponPartsItem, position, Quaternion.identity) as DroppedItem;
            if (partsItem != null) partsItem.SetItem(item);
            return partsItem;
        }
    }
}
