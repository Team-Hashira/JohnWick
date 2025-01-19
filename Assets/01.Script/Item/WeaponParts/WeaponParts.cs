using Hashira.Core.StatSystem;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items.WeaponPartsSystem
{
    public class WeaponParts : Item, IStatable
    {
        public WeaponPartsSO WeaponPartsSO { get; private set; }

        public StatDictionary StatDictionary => WeaponPartsSO.StatDictionary;

        protected Weapon _weapon;

        //가장 처음 만들어질 때 한번
        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);

            WeaponPartsSO = itemSO as WeaponPartsSO;
        }

        public virtual void Equip(Weapon weapon)
        {
            _weapon = weapon;
            foreach (StatElement stat in WeaponPartsSO.StatDictionary.GetElements())
                _weapon.StatDictionary[stat.elementSO].AddModify(WeaponPartsSO.itemName, stat.Value, EModifyMode.Add);
            Debug.Log($"{WeaponPartsSO.itemDisplayName} 장착!");
        }
        public virtual void UnEquip()
        {
            foreach (StatElement stat in WeaponPartsSO.StatDictionary.GetElements())
                _weapon.StatDictionary[stat.elementSO].RemoveModify(WeaponPartsSO.itemName, EModifyMode.Add);
            Debug.Log($"{WeaponPartsSO.itemDisplayName} 장착해제!");
        }
        public virtual void PartsUpdate()
        {

        }
    }
}
