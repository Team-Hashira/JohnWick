using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using Hashira.Items.WeaponPartsSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class Weapon : Item, ICloneable, IStatable
    {
        public WeaponSO WeaponSO { get; private set; }
        protected EntityWeapon _EntityWeapon { get; private set; }

        //Parts
        private Dictionary<EWeaponPartsType, WeaponParts> _partsSlotDictionary = new Dictionary<EWeaponPartsType, WeaponParts>();
        public event Action<EWeaponPartsType, WeaponParts> OnPartsChanged;

        //Stat
        private List<StatElement> _overrideStatElementList = new List<StatElement>();
        private StatBaseSO _baseStat;
        public StatDictionary StatDictionary { get; private set; }

        private int _entityDamage;

        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);

            WeaponSO = itemSO as WeaponSO;

            if (WeaponSO.baseStat == null) Debug.LogError("BaseStat is null with WeaponSO");
            else
            {
                _baseStat = GameObject.Instantiate(WeaponSO.baseStat);
                _overrideStatElementList = WeaponSO.overrideStatElementList;

                StatDictionary = new StatDictionary(_overrideStatElementList, _baseStat);
            }

            foreach (EWeaponPartsType partsType in WeaponSO.partsEquipPosDict.Keys)
                _partsSlotDictionary.Add(partsType, null);
        }

        //장착
        public virtual void Equip(EntityWeapon entityWeapon)
        {
            _EntityWeapon = entityWeapon;
        }
        //장착중
        public virtual void WeaponUpdate()
        {
            foreach (WeaponParts parts in _partsSlotDictionary.Values)
            {
                parts?.PartsUpdate();
            }
        }
        //장착해제
        public virtual void UnEquip()
        {
            _EntityWeapon = null;
        }

        public virtual void Attack(int damage, bool isDown)
        {
            _entityDamage = damage;
        }

        public virtual int CalculateDamage() { return _entityDamage + StatDictionary["AttackPower"].IntValue; }

        public virtual object Clone()
        {
            Weapon clonedWeapon = MemberwiseClone() as Weapon;
            clonedWeapon.WeaponSO = WeaponSO;
            return clonedWeapon;
        }

        public WeaponParts EquipParts(EWeaponPartsType eWeaponPartsType, WeaponParts parts)
        {
            //장착 불가능하면 그대로 반환
            if (_partsSlotDictionary.ContainsKey(eWeaponPartsType) == false) return parts;

            WeaponParts prevPartsSO = _partsSlotDictionary[eWeaponPartsType];

            //교환
            _partsSlotDictionary[eWeaponPartsType]?.UnEquip();
            _partsSlotDictionary[eWeaponPartsType] = parts;
            _partsSlotDictionary[eWeaponPartsType]?.Equip(this);

            OnPartsChanged?.Invoke(eWeaponPartsType, parts);

            return prevPartsSO;
        }

        public WeaponParts GetParts(EWeaponPartsType eWeaponPartsType)
        {
            if (_partsSlotDictionary.TryGetValue(eWeaponPartsType, out WeaponParts weaponParts))
                return weaponParts;
            else
                return null;
        }
        public bool TryGetParts(EWeaponPartsType eWeaponPartsType, out WeaponParts weaponParts)
        {
            if (_partsSlotDictionary.TryGetValue(eWeaponPartsType, out WeaponParts parts))
            {
                weaponParts = parts;
                return weaponParts != null;
            }
            weaponParts = null;
            return false;
        }
    }
}
