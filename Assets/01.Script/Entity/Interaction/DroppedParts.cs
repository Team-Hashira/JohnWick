using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
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

            EntityGunWeapon weaponHolder = entity.GetEntityComponent<EntityGunWeapon>();

            if (weaponHolder.CurrentWeapon != null && weaponHolder.CurrentWeapon is GunWeapon gunWeapon)
            {
                WeaponParts weaponParts = gunWeapon.EquipParts(_partsSO.partsType, _parts);
                if (weaponParts != _parts)
                {
                    if (weaponParts != null) ItemDropUtility.DroppedItem(weaponParts, entity.transform.position);
                    InteractionSucces();
                    this.Push();
                }
            }
        }

        public override void SetItemData()
        {
            _ItemDataTrm.gameObject.SetActive(true);
            if (_entity.TryGetEntityComponent(out EntityGunWeapon entityWeapon) &&
                entityWeapon.CurrentWeapon != null && entityWeapon.CurrentWeapon is GunWeapon gunWeapon &&
                gunWeapon.TryGetParts(_partsSO.partsType, out WeaponParts comparisonParts))
            {
                _ComparisonItemDataTrm.gameObject.SetActive(true);
                _itemData.SetItem(_parts, comparisonParts.WeaponPartsSO);
                _comparisonItemData.SetItem(comparisonParts);
            }
            else
            {
                _ComparisonItemDataTrm.gameObject.SetActive(false);
                _itemData.SetItem(_parts);
            }
        }
    }
}
