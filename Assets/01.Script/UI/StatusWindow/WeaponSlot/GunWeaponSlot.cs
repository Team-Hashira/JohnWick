using System.Collections.Generic;
using System.Linq;
using Hashira.Items.WeaponPartsSystem;
using Hashira.Items.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class GunWeaponSlot : MonoBehaviour, IWeaponSlot
    {
        [SerializeField] private PartSlot _partSlotPrefab; 
        public Weapon baseWeapon;
        [SerializeField] private GunWeaponSlotIcon _icon;
        private readonly List<PartSlot> _partSlotList = new List<PartSlot>();

        public int SlotIndex { get; set; }

        public void HandleWeaponChanged(Weapon weapon)
        {
            if(baseWeapon != null)
                baseWeapon.OnPartsChanged -= HandleParsChanged;
            
            // 기존에 있던 UI 삭제
            foreach (var partSlot in _partSlotList)
            {
                Destroy(partSlot.gameObject);
            }
            _partSlotList.Clear();
            
            // 새로 추가
            if (weapon != null)
            {
                foreach (var posPair in weapon.WeaponSO.partsEquipPosDict)
                {
                    var partSlot = AddPartSlot(posPair.Key, posPair.Value);
                    partSlot.Init(this, weapon.GetParts(posPair.Key));
                }    
            }
            
            baseWeapon = weapon;
            _icon.Init(this);
            
            if(baseWeapon != null)
                baseWeapon.OnPartsChanged += HandleParsChanged;
        }

        private void HandleParsChanged(EWeaponPartsType partsType, WeaponParts weaponParts)
        {
            var partSlot = _partSlotList.FirstOrDefault(x => x.partType == partsType);
            partSlot?.Init(this, weaponParts);
        }
        
        private PartSlot AddPartSlot(EWeaponPartsType partType, Vector2 position)
        {
            PartSlot partSlot = Instantiate(_partSlotPrefab, transform);
            partSlot.partType = partType;
            
            // 위치 조정
            RectTransform partSlotRectTrm = partSlot.transform as RectTransform;
            if (partSlotRectTrm != null) partSlotRectTrm.anchoredPosition = position;

            _partSlotList.Add(partSlot);

            return partSlot;
        }
    }
}
