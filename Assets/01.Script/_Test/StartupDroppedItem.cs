using Crogen.CrogenPooling;
using Hashira.Entities.Interacts;
using Hashira.Items;
using UnityEngine;

namespace Hashira
{
    public class StartupDroppedItem : StartupPool<DroppedItem>
    {
        [SerializeField] private ItemSO _itemSO;
        protected override void PopObjectSetting(DroppedItem popedObject)
        {
            popedObject.SetItem(_itemSO);
        }
    }
}
