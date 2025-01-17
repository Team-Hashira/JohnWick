using Hashira.Entities.Components;
using Hashira.Weapons;
using System;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class DroppedWeapon : DroppedItem
    {
        [SerializeField] private WeaponSO _weaponSO;
        private Weapon _weapon;

        protected override void Awake()
        {
            base.Awake();
            SetWeapon(_weaponSO.GetWeaponClass());
        }

        private void SetWeapon(Weapon weapon)
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
    }
}
