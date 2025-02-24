using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using Hashira.UI.InGame;
using System;
using System.Collections.Generic;
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
            //else if(item is SubItem subItem)
            //{
            //    _weapon = subItem;
            //    _weaponSO = subItem.SubItemSO;
            //}

            base.SetItem(item);
        }

        public override void Interaction(Entity entity)
        {
            Weapon weapon = null;
            if (_weapon is GunWeapon gunWeapon)
            {
                EntityWeaponHolder weaponHolder = entity.GetEntityComponent<EntityWeaponHolder>();
                weapon = weaponHolder?.EquipItem(gunWeapon);
            }

            if (weapon != null) ItemDropUtility.DroppedItem(weapon, entity.transform.position);
            InteractionSucces();
            this.Push();

            base.Interaction(entity);
        }

        public override void SetItemData()
        {
            base.SetItemData();
            ItemDataUIController itemDataController
                = UIManager.Instance.PopupUIActive<ItemDataUIController>(EPopupUIName.ItemDataUI, true)[0];
            if (_weapon is GunWeapon && _entity.TryGetEntityComponent(out EntityWeaponHolder entityWeapon))
            {
                itemDataController.SetItem(_weapon, entityWeapon.CurrentWeapon);
            }
            else
            {
                itemDataController.SetItem(_weapon);
            }
        }
    }
}
