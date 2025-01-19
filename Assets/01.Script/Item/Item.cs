using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items
{
    public class Item : ICloneable
    {
        public ItemSO ItemSO { get; private set; }

        public virtual object Clone() => MemberwiseClone();

        public virtual void Init(ItemSO itemSO)
        {
            ItemSO = itemSO;
        }
    }
}
 