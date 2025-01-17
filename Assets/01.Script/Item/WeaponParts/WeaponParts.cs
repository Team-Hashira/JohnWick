using System;
using UnityEngine;

namespace Hashira.Weapons
{
    public class WeaponParts : ICloneable
    {
        public WeaponPartsSO WeaponPartsSO { get; private set; }

        //���� ó�� ������� �� �ѹ�
        public void Init(WeaponPartsSO weaponPartsSO)
        {
            WeaponPartsSO = weaponPartsSO;
        }

        public virtual void Equip(Weapon weapon)
        {
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
