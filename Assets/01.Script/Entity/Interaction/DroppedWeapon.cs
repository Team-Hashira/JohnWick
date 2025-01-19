using Hashira.Entities.Components;
using Hashira.Items.WeaponPartsSystem;
using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class DroppedWeapon : DroppedItem
    {
        [Header("==========DroppedWeapon setting==========")]
        [SerializeField] private WeaponSO _weaponSO;
        private Weapon _weapon;

        protected override void Awake()
        {
            base.Awake();
            SetWeapon(_weaponSO.GetWeaponClass());
        }

        public void SetWeapon(Weapon weapon)
        {
            _weapon = weapon;
            _weaponSO = weapon.WeaponSO;
            SetItemSO(_weaponSO);
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            EntityWeapon weaponHolder = entity.GetEntityComponent<EntityWeapon>();
            Weapon weapon = weaponHolder.EquipWeapon(_weapon);
            if (weapon != null) SetWeapon(weapon);
            else Destroy(gameObject);
        }

        public override void SetItemData()
        {
            _ItemDataTrm.gameObject.SetActive(true);
            if (_entity.TryGetEntityComponent(out EntityWeapon entityWeapon) &&
                entityWeapon.CurrentWeapon != null)
            {
                _ComparisonItemDataTrm.gameObject.SetActive(true);
                _itemData.SetItem(_weapon, entityWeapon.CurrentWeapon);
                _comparisonItemData.SetItem(entityWeapon.CurrentWeapon);
            }
            else
            {
                _itemData.SetItem(_weapon);
            }
        }
    }
}
