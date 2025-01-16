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

        public WeaponPartsSO EquipParts(EWeaponPartsType eWeaponPartsType, WeaponPartsSO partsSO)
        {
            WeaponPartsSO prevPartsSO = _partsSlotDictionary[eWeaponPartsType];

            //���⼭ Parts�� Equip, UnEquip�����ְ�
            //PartsSO������ WeaponSOó�� PartsŬ������ ���� ������ �ű��ִ� �Լ��� ��������ִ� ������.
            //Parts�� ���� �����ϰ� �������� ������ ������ �ʿ䰡 �ְڴ�Tiqkf(ex. ��ź �߻���� ��Ÿ���� �ٲ�ٳ��� �ʱ�ȭ �Ǵ°� ����)
            //���� �׷� Parts�� SO�� ���ϰ� Ŭ������ DroppedItem�� �����ؾ��ϴ°ų�? ��������������������������������
            //1�� 17�� �赿���� ������ ��� �� �ܴ٤� ����
            _partsSlotDictionary[eWeaponPartsType] = partsSO;
            OnPartsChanged?.Invoke(eWeaponPartsType, partsSO);

            return prevPartsSO;
        }
    }
}
