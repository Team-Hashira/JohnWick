using Hashira.Core.StatSystem;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using Hashira.UI.DragSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class PartSlot : MonoBehaviour, ISlot, ISelectableObject
    {
        [SerializeField] private PartSlotIcon _icon;
        public bool isAllType = false;
        public EWeaponPartsType partType;
        public GunWeaponSlot WeaponSlot { get; private set; }
        public Item Item { get; set; }

        public event Action OnChangedPartsEvent;
        private RectTransform _rectTransform;

        private Outline _outline;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
            _rectTransform = transform as RectTransform;
        }

        public void Init(GunWeaponSlot weaponSlot, WeaponParts weaponPart)
        {
            WeaponSlot = weaponSlot;
            Item = weaponPart;
            _icon.Init(this);
        }

        public void EquipParts(EWeaponPartsType eWeaponPartsType, WeaponParts parts)
        {
            if (isAllType)
            {
                Item = parts;
                if (parts != null) partType = parts.WeaponPartsSO.partsType;
                _icon.Init(this);

                OnChangedPartsEvent?.Invoke();
                return;
            }

            OnChangedPartsEvent?.Invoke();
            (WeaponSlot.Item as GunWeapon).EquipParts(eWeaponPartsType, parts);
        }

        public WeaponParts UnEquipParts(EWeaponPartsType eWeaponPartsType)
        {
            if (isAllType)
            {
                var temp = Item;
                Item = null;
                _icon.Init(this);

                OnChangedPartsEvent?.Invoke();
                return temp as WeaponParts;
            }

            OnChangedPartsEvent?.Invoke();
            return (WeaponSlot.Item as GunWeapon).UnEquipParts(eWeaponPartsType);
        }

        public void OnSelectStart()
        {
            Debug.Log("파츠 선택");
            _outline.enabled = true;
        }

        public void OnSelectEnd()
        {
            _outline.enabled = false;
        }
    }
}
