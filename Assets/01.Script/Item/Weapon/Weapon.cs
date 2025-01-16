using Hashira.Entities.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Weapons
{
    public class Weapon : ICloneable
    {
        public WeaponSO WeaponSO { get; private set; }

        protected EntityWeapon _EntityWeapon { get; private set; }

        private Dictionary<EWeaponPartsType, WeaponPartsSO> _partsSlotDictionary = new Dictionary<EWeaponPartsType, WeaponPartsSO>();

        public event Action<EWeaponPartsType, WeaponPartsSO> OnPartsChanged;

        public void Init(WeaponSO weaponSO)
        {   
            WeaponSO = weaponSO;
            foreach (EWeaponPartsType partsType in Enum.GetValues(typeof(EWeaponPartsType)))
                _partsSlotDictionary.Add(partsType, null);
        }

        //¿Â¬¯
        public virtual void Equip(EntityWeapon entityWeapon)
        {
            _EntityWeapon = entityWeapon;
        }
        //¿Â¬¯¡ﬂ
        public virtual void WeaponUpdate()
        {

        }
        //¿Â¬¯«ÿ¡¶
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

        public WeaponPartsSO SwapParts(EWeaponPartsType eWeaponPartsType, WeaponPartsSO partsSO)
        {
            Debug.Log(eWeaponPartsType);
            WeaponPartsSO prevPartsSO = _partsSlotDictionary[eWeaponPartsType];

            _partsSlotDictionary[eWeaponPartsType] = partsSO;
            OnPartsChanged?.Invoke(eWeaponPartsType, partsSO);

            return prevPartsSO;
        }
    }
}
