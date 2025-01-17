using Hashira.Core.StatSystem;
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

        private Dictionary<EWeaponPartsType, WeaponParts> _partsSlotDictionary = new Dictionary<EWeaponPartsType, WeaponParts>();

        public event Action<EWeaponPartsType, WeaponParts> OnPartsChanged;

        private List<StatElement> _overrideStatElementList = new List<StatElement>();
        private StatBaseSO _baseStat;


        public void Init(WeaponSO weaponSO)
        {   
            WeaponSO = weaponSO;
            if (weaponSO.baseStat == null) Debug.LogError("BaseStat is null with WeaponSO");
            else _baseStat = GameObject.Instantiate(weaponSO.baseStat);
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
            foreach (WeaponParts parts in _partsSlotDictionary.Values)
            {
                parts?.PartsUpdate();
            }
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

        public WeaponParts EquipParts(EWeaponPartsType eWeaponPartsType, WeaponParts parts)
        {
            WeaponParts prevPartsSO = _partsSlotDictionary[eWeaponPartsType];

            //±≥»Ø
            _partsSlotDictionary[eWeaponPartsType]?.UnEquip();
            _partsSlotDictionary[eWeaponPartsType] = parts;
            _partsSlotDictionary[eWeaponPartsType]?.Equip(this);

            OnPartsChanged?.Invoke(eWeaponPartsType, parts);

            return prevPartsSO;
        }
    }
}
