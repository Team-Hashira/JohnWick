using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items
{
    public abstract class Item : ICloneable, IStatable
    {
        public ItemSO ItemSO { get; private set; }
        public virtual StatDictionary StatDictionary { get; protected set; }

        public virtual object Clone() => MemberwiseClone();

        public abstract void Equip(EntityItemHolder holder);
        public abstract void UnEquip();

        /// <summary>
        /// Item이 선택된 상태에서만 돌아갑니다
        /// </summary>
        public abstract void ItemUpdate();

        public virtual void Init(ItemSO itemSO)
        {
            ItemSO = itemSO;
        }
    }
}
 