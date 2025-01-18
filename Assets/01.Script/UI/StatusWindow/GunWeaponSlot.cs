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
        [SerializeField] private Image _iconImage;
        private Weapon _baseWeapon;

        private readonly List<PartSlot> _partSlotList = new List<PartSlot>();

        public void HandleWeaponChanged(Weapon weapon)
        {
            _baseWeapon = weapon;
            _iconImage.sprite = weapon.WeaponSO.itemSprite;

            // 기존에 있던 UI 삭제
            _partSlotList.ForEach(partSlot => Destroy(partSlot.gameObject));
            _partSlotList.Clear();
            
            // 새로 추가
            foreach (var posPair in weapon.WeaponSO.partsEquipPosDict)
                AddPartSlot(posPair.Key, posPair.Value);
        }

        public void OnChangedPart(WeaponPartsSO weaponPartsSO)
        {
            var partSlot = _partSlotList.FirstOrDefault(x => x.partType == weaponPartsSO.partsType);
            partSlot?.Init(weaponPartsSO);
        }
        
        private void AddPartSlot(EWeaponPartsType partType, Vector2 position)
        {
            PartSlot partSlot = Instantiate(_partSlotPrefab, transform);
            partSlot.partType = partType;
            
            // 위치 조정
            RectTransform partSlotRectTrm = partSlot.transform as RectTransform;
            partSlotRectTrm.anchoredPosition = position;
            
            _partSlotList.Add(partSlot);
        }
    }
}
