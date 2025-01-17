using Hashira.Core.StatSystem;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items.WeaponPartsSystem
{
    public class WeaponParts : Item, ICloneable, IStatable
    {
        public WeaponPartsSO WeaponPartsSO { get; private set; }

        public StatDictionary StatDictionary => WeaponPartsSO.StatDictionary;

        protected Weapon _weapon;

        //���� ó�� ������� �� �ѹ�
        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);

            WeaponPartsSO = itemSO as WeaponPartsSO;
        }

        public virtual void Equip(Weapon weapon)
        {
            _weapon = weapon;
            Debug.Log($"{WeaponPartsSO.itemDisplayName} ����!");
        }
        public virtual void UnEquip()
        {
            Debug.Log($"{WeaponPartsSO.itemDisplayName} ��������!");
        }
        public virtual void PartsUpdate()
        {

        }

        public object Clone()
        {
            return MemberwiseClone() as WeaponParts;
        }
    }
}
