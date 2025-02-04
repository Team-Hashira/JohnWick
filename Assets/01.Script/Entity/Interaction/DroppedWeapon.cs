using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class DroppedWeapon : DroppedItem
    {
        [Header("==========DroppedWeapon setting==========")]
        [SerializeField] private WeaponSO _weaponSO;
        [SerializeField] private PartsRenderer _partsRenderer;
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
            _partsRenderer.SetGun(item as GunWeapon);
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
            if (_entity.TryGetEntityComponent(out EntityWeapon entityWeapon))
            {
                if (_weapon is MeleeWeapon)
                {
                    if (entityWeapon.Weapons[2] != null)
                    {
                        _ComparisonItemDataTrm.gameObject.SetActive(true);
                        _itemData.SetItem(_weapon, entityWeapon.Weapons[2]);
                        _comparisonItemData.SetItem(entityWeapon.Weapons[2]);
                    }
                    else
                    {
                        _ComparisonItemDataTrm.gameObject.SetActive(false);
                        _itemData.SetItem(_weapon);
                    }
                }
                else if (entityWeapon.CurrentWeapon != null)
                {
                    _ComparisonItemDataTrm.gameObject.SetActive(true);
                    _itemData.SetItem(_weapon, entityWeapon.CurrentWeapon);
                    _comparisonItemData.SetItem(entityWeapon.CurrentWeapon);
                }
                else
                {
                    _ComparisonItemDataTrm.gameObject.SetActive(false);
                    _itemData.SetItem(_weapon);
                }
            }
            else
            {
                _itemData.SetItem(_weapon);
            }
        }
    }
}
