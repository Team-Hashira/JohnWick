using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.Items
{
    public class Item
    {
        public ItemSO ItemSO { get; private set; }

        public virtual void Init(ItemSO itemSO)
        {
            ItemSO = itemSO;
        }
    }
}
 