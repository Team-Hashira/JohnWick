using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using Hashira.UI.StatusWindow;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class DroppedParts : DroppedItem
    {
        [Header("==========DroppedParts setting==========")]
        [SerializeField] private PartsSO _partsSO;
        private WeaponParts _parts;

        protected override void Awake()
        {
            base.Awake();
            if (_partsSO == null) return;
            SetItem(_partsSO);
        }

        public override void SetItem(Item item)
        {
            _parts = item as WeaponParts;
            _partsSO = _parts.WeaponPartsSO;
            base.SetItem(item);
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            EntityWeaponHolder weaponHolder = entity.GetEntityComponent<EntityWeaponHolder>();

            if (weaponHolder.CurrentItem != null && weaponHolder.CurrentItem is GunWeapon gunWeapon)
            {
                if (gunWeapon.CheckWeaponPartsType(_partsSO.partsType))
                {
                    if (gunWeapon.CheckPartsSlotEmpty(_partsSO.partsType))
                    {
                        WeaponParts weaponParts = gunWeapon.EquipParts(_partsSO.partsType, _parts);
                        this.Push();
                        return;
                    }
                }
                if (Inventory.Instance.AddPart(_parts))
                {
                    InteractionSucces();
                    this.Push();
                }
            }
        }

        public override void SetItemData()
        {
            base.SetItemData();
            if (_entity.TryGetEntityComponent(out EntityWeaponHolder entityWeapon) &&
                entityWeapon.CurrentItem != null && entityWeapon.CurrentItem is GunWeapon gunWeapon &&
                gunWeapon.TryGetParts(_partsSO.partsType, out WeaponParts comparisonParts))
            {
                _itemDataController.SetItem(_parts, comparisonParts);
            }
            else
            {
                _itemDataController.SetItem(_parts);
            }
        }
    }
}
