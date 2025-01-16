using Hashira.Entities.Components;
using System;
using UnityEngine;

namespace Hashira.Weapons
{
    public class Weapon : ICloneable
    {
        public WeaponSO WeaponSO { get; private set; }

        protected EntityWeapon _EntityWeapon { get; private set; }

        public void Init(WeaponSO weaponSO)
        {   
            WeaponSO = weaponSO;
        }

        //����
        public virtual void Equip(EntityWeapon entityWeapon)
        {
            _EntityWeapon = entityWeapon;
        }
        //������
        public virtual void WeaponUpdate()
        {

        }
        //��������
        public virtual void UnEquip()
        {
            _EntityWeapon = null;
        }

        public virtual void Attack(int damage, bool isDown)
        {

        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
