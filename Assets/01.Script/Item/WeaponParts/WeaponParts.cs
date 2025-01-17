using System;
using UnityEngine;

namespace Hashira.Weapons
{
    public class WeaponParts : ICloneable
    {
        public WeaponPartsSO WeaponPartsSO { get; private set; }

        protected Weapon _weapon;


        //���� ó�� ������� �� �ѹ�
        public void Init(WeaponPartsSO weaponPartsSO)
        {
            WeaponPartsSO = weaponPartsSO;
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
            return MemberwiseClone();
        }
    }
}
