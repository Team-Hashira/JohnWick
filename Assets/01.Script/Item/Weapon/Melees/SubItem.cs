using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Combat;
using Hashira.Entities.Components;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items.SubItems
{
    public class SubItem : Item
    {
        public SubItemSO SubItemSO { get; private set; }
        public EntitySubItemHolder EntitySubItemHolder { get; private set; }

        public override void Equip(EntityItemHolder entityItemHolder) 
        {
            EntitySubItemHolder = entityItemHolder as EntitySubItemHolder;
        }

        public override void ItemUpdate()
        {

        }

        public override void UnEquip() { }

        public virtual void Use() { }

        public override object Clone()
        {
            SubItemSO = ItemSO as SubItemSO;
            return base.Clone();
        }
    }
}
