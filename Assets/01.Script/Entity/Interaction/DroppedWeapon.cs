using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Items;
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
            if (_weaponSO == null) return;
            SetItem(_weaponSO);
        }

        public override void SetItem(Item item)
        {
            _weapon = item as Weapon;
            _weaponSO = _weapon.WeaponSO;
            base.SetItem(item);
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            EntityWeapon weaponHolder = entity.GetEntityComponent<EntityWeapon>();
            Weapon weapon = weaponHolder.EquipWeapon(_weapon);
            if (weapon != null) SetItem(weapon);
            else this.Push();
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
