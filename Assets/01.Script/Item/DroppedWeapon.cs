using Hashira.Entities.Components;
using Hashira.Weapons;
using System;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class DroppedWeapon : KeyInteractObject
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private WeaponSO _weaponSO;
        [SerializeField] private Weapon _weapon;

        protected override void Awake()
        {
            base.Awake();
            Init(_weaponSO.GetWeaponClass());
        }

        public void Init(Weapon weapon)
        {
            _weapon = weapon;
            _weaponSO = weapon.WeaponSO;
            _spriteRenderer.sprite = weapon.WeaponSO.itemIcon;
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            EntityWeapon weaponHolder = entity.GetEntityComponent<EntityWeapon>();
            Weapon weapon = weaponHolder.EquipWeapon(_weapon);
            if (weapon != null)
                Init(weapon);
            else
                Destroy(gameObject);
        }
    }
}
