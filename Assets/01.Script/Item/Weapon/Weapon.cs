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

        //장착
        public virtual void Equip(EntityWeapon entityWeapon)
        {
            _EntityWeapon = entityWeapon;
        }
        //장착중
        public virtual void WeaponUpdate()
        {

        }
        //장착해제
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

            //여기서 Parts들 Equip, UnEquip시켜주고
            //PartsSO에서는 WeaponSO처럼 Parts클래스를 따로 가지고 거기있는 함수를 실행시켜주는 구조로.
            //Parts는 따로 저장하고 여러개가 제각각 존재할 필요가 있겠다Tiqkf(ex. 유탄 발사기의 쿨타임이 바꿨다끼면 초기화 되는걸 방지)
            //ㅆㅂ 그럼 Parts도 SO로 못하고 클래스로 DroppedItem에 저장해야하는거네? ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ
            //1월 17일 김동률은 ㅈ뺑이 까라 난 잔다ㅋ ㅆㅂ
            _partsSlotDictionary[eWeaponPartsType] = partsSO;
            OnPartsChanged?.Invoke(eWeaponPartsType, partsSO);

            return prevPartsSO;
        }
    }
}
