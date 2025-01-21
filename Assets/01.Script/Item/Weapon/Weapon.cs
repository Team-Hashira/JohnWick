using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using Hashira.Items.PartsSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class Weapon : Item, IStatable
    {
        public WeaponSO WeaponSO { get; private set; }
        protected EntityWeapon _EntityWeapon { get; private set; }

        //Parts
        private readonly Dictionary<EWeaponPartsType, WeaponParts> _partsSlotDictionary = new Dictionary<EWeaponPartsType, WeaponParts>();
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
            _partsSlotDictionary.Clear();
            if (WeaponSO.baseStat == null) Debug.LogError("BaseStat is null with WeaponSO");
            else
            {
                _baseStat = GameObject.Instantiate(WeaponSO.baseStat);
                _overrideStatElementList = WeaponSO.overrideStatElementList;

                StatDictionary = new StatDictionary(_overrideStatElementList, _baseStat);
            }

            foreach (EWeaponPartsType partsType in WeaponSO.partsEquipPosDict.Keys)
            {
                _partsSlotDictionary.Add(partsType, null);
            }
        }

        //����
        public virtual void Equip(EntityWeapon entityWeapon)
        {
            _EntityWeapon = entityWeapon;
        }
        //������
        public virtual void WeaponUpdate()
        {
            foreach (WeaponParts parts in _partsSlotDictionary.Values)
            {
                parts?.PartsUpdate();
            }
        }
        //��������
        public virtual void UnEquip()
        {
            _EntityWeapon = null;
        }

        public virtual void Attack(int damage, bool isDown)
        {
            _entityDamage = damage;
        }

        public virtual int CalculateDamage() { return _entityDamage + StatDictionary["AttackPower"].IntValue; }

        public WeaponParts EquipParts(EWeaponPartsType eWeaponPartsType, WeaponParts parts)
        {
            //���� �Ұ����ϸ� �״�� ��ȯ
            if (_partsSlotDictionary.ContainsKey(eWeaponPartsType) == false) return parts;

            WeaponParts prevPartsSO = _partsSlotDictionary[eWeaponPartsType];

            //��ȯ
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
