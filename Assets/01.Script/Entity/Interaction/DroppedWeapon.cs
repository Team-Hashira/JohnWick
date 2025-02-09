using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using System;
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
            _partsRenderer.Init();

            if (_weaponSO == null) return;
            SetItem(_weaponSO);
        }

        public override void SetItem(Item item)
        {
            if (item is GunWeapon gunWeapon)
            {
                _weapon = gunWeapon;
                _weaponSO = gunWeapon.GunSO;
                gunWeapon.SetPartsRenderer(_partsRenderer);
                _partsRenderer.SetGun(gunWeapon);
            }
            else if(item is MeleeWeapon meleeWeapon)
            {
                _weapon = meleeWeapon;
                _weaponSO = meleeWeapon.MeleeSO;
            }

            base.SetItem(item);
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            Weapon weapon = null;
            if (_weapon is GunWeapon gunWeapon)
            {
                EntityGunWeapon weaponHolder = entity.GetEntityComponent<EntityGunWeapon>();
                weapon = weaponHolder?.EquipWeapon(gunWeapon);
            }
            else if (_weapon is MeleeWeapon meleeWeapon)
            {
                EntityMeleeWeapon weaponHolder = entity.GetEntityComponent<EntityMeleeWeapon>();
                weapon = weaponHolder?.EquipWeapon(meleeWeapon);
            }

            if (weapon != null) SetItem(weapon);
            else this.Push();
        }

        public override void SetItemData()
        {
            _ItemDataTrm.gameObject.SetActive(true);
            if (_weapon is GunWeapon && _entity.TryGetEntityComponent(out EntityGunWeapon entityWeapon))
            {
                if (entityWeapon.CurrentWeapon != null)
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
            else if (_weapon is MeleeWeapon && _entity.TryGetEntityComponent(out EntityMeleeWeapon entityMeleeWeapon))
            {
                if (entityMeleeWeapon.CurrentWeapon != null)
                {
                    _ComparisonItemDataTrm.gameObject.SetActive(true);
                    _itemData.SetItem(_weapon, entityMeleeWeapon.CurrentWeapon);
                    _comparisonItemData.SetItem(entityMeleeWeapon.CurrentWeapon);
                }
                else
                {
                    _ComparisonItemDataTrm.gameObject.SetActive(false);
                    _itemData.SetItem(_weapon);
                }
            }
            else
            {
                _ComparisonItemDataTrm.gameObject.SetActive(false);
                _itemData.SetItem(_weapon);
            }
        }
    }
}
