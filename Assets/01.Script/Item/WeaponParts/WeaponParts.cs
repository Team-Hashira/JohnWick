using System;
using UnityEngine;

namespace Hashira.Weapons
{
    public class WeaponParts : ICloneable
    {
        public WeaponPartsSO WeaponPartsSO { get; private set; }

        //가장 처음 만들어질 때 한번
        public void Init(WeaponPartsSO weaponPartsSO)
        {
            WeaponPartsSO = weaponPartsSO;
        }

        public virtual void Equip(Weapon weapon)
        {
            Debug.Log($"{WeaponPartsSO.itemDisplayName} 장착!");
        }
        public virtual void UnEquip()
        {
            Debug.Log($"{WeaponPartsSO.itemDisplayName} 장착해제!");
        }
        public virtual void PartsUpdate()
        {

        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
