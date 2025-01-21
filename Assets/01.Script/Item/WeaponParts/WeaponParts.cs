using Hashira.Core.StatSystem;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class WeaponParts : Item, IStatable
    {
        public PartsSO WeaponPartsSO { get; private set; }

        public StatDictionary StatDictionary => WeaponPartsSO.StatDictionary;

        protected Weapon _weapon;

        //���� ó�� ������� �� �ѹ�
        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);

            WeaponPartsSO = itemSO as PartsSO;
        }

        public virtual void Equip(Weapon weapon)
        {
            _weapon = weapon;
            foreach (StatElement stat in WeaponPartsSO.StatDictionary.GetElements())
                _weapon.StatDictionary[stat.elementSO].AddModify(WeaponPartsSO.itemName, stat.Value, EModifyMode.Add);
            Debug.Log($"{WeaponPartsSO.itemDisplayName} ����!");
        }
        public virtual void UnEquip()
        {
            foreach (StatElement stat in WeaponPartsSO.StatDictionary.GetElements())
                _weapon.StatDictionary[stat.elementSO].RemoveModify(WeaponPartsSO.itemName, EModifyMode.Add);
            Debug.Log($"{WeaponPartsSO.itemDisplayName} ��������!");
        }
        public virtual void PartsUpdate()
        {

        }
    }
}
