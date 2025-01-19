using Hashira.Entities.Components;
using Hashira.Items.WeaponPartsSystem;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class DroppedParts : DroppedItem
    {
        [Header("==========DroppedParts setting==========")]
        [SerializeField] private WeaponPartsSO _partsSO;
        private WeaponParts _parts;

        protected override void Awake()
        {
            base.Awake();
            SetParts(_partsSO.GetWeaponPartsClass());
        }

        public void SetParts(WeaponParts parts)
        {
            _parts = parts;
            _partsSO = parts.WeaponPartsSO;
            SetItemSO(_partsSO);
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            EntityWeapon weaponHolder = entity.GetEntityComponent<EntityWeapon>();

            if (weaponHolder.CurrentWeapon == null) return;

            WeaponParts weaponParts = weaponHolder.CurrentWeapon.EquipParts(_partsSO.partsType, _parts);
            if (weaponParts != null) SetParts(weaponParts);
            else Destroy(gameObject);
        }

        public override void SetItemData()
        {
            _ItemDataTrm.gameObject.SetActive(true);
            if (_entity.TryGetEntityComponent(out EntityWeapon entityWeapon) &&
                entityWeapon.CurrentWeapon != null &&
                entityWeapon.CurrentWeapon.TryGetParts(_partsSO.partsType, out WeaponParts comparisonParts))
            {
                _ComparisonItemDataTrm.gameObject.SetActive(true);
                _itemData.SetItem(_parts, comparisonParts.WeaponPartsSO);
                _comparisonItemData.SetItem(comparisonParts);
            }
            else
            {
                _itemData.SetItem(_parts);
            }
        }
    }
}
