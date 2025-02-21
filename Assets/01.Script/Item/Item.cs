using Hashira.Core.StatSystem;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items
{
    public class Item : ICloneable, IStatable
    {
        public ItemSO ItemSO { get; private set; }
        public virtual StatDictionary StatDictionary { get; protected set; }

        public virtual object Clone() => MemberwiseClone();

        public virtual void Init(ItemSO itemSO)
        {
            ItemSO = itemSO;
        }
    }
}
 