using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Items
{
    public class ItemDropUtility : MonoBehaviour
    {
        public static DroppedItem DroppedItem(Item item, Vector3 position)
        {
            DroppedItem droppedItem = PopCore.Pop(ItemPoolType.WeaponItem, position, Quaternion.identity).gameObject.GetComponent<DroppedItem>();
            droppedItem.SetItem(item);
            return droppedItem;
        }
    }
}
