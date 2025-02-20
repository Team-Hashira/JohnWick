using Hashira.Items.PartsSystem;
using Hashira.UI.DragSystem;
using System;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class PartSlot : MonoBehaviour, ISelectableObject
    {
        [SerializeField] private PartSlotIcon _icon;
        public bool isAllType = false;
        public EWeaponPartsType partType;
        public WeaponParts BasePart { get; private set; }
        public GunWeaponSlot WeaponSlot { get; private set; }
        public event Action OnChangedPartsEvent;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
        }

        public void Init(GunWeaponSlot weaponSlot, WeaponParts weaponPart)
        {
            WeaponSlot = weaponSlot;
            BasePart = weaponPart;
            _icon.Init(this);
        }

        public void EquipParts(EWeaponPartsType eWeaponPartsType, WeaponParts parts)
        {
            if (isAllType)
            {
                BasePart = parts;
                if (parts != null) partType = parts.WeaponPartsSO.partsType;
                _icon.Init(this);

                OnChangedPartsEvent?.Invoke();
                return;
            }

            OnChangedPartsEvent?.Invoke();
            WeaponSlot.GunWeapon.EquipParts(eWeaponPartsType, parts);
        }

        public WeaponParts UnEquipParts(EWeaponPartsType eWeaponPartsType)
        {

            if (isAllType)
            {
                var temp = BasePart;
                BasePart = null;
                _icon.Init(this);

                OnChangedPartsEvent?.Invoke();
                return temp;
            }

            OnChangedPartsEvent?.Invoke();
            return WeaponSlot.GunWeapon.UnEquipParts(eWeaponPartsType);
        }

        public void OnSelectStart()
        {
            if (BasePart == null) return;
            WeaponDescriptionContainer.Instance.Open(BasePart.WeaponPartsSO, _rectTransform);
        }

        public void OnSelectEnd()
        {
            WeaponDescriptionContainer.Instance.Close();
        }
    }
}
