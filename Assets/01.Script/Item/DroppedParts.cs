using Hashira.Entities.Components;
using Hashira.Entities;
using Hashira.Entities.Interacts;
using Hashira.Weapons;
using UnityEngine;
using TMPro;

namespace Hashira.Entities.Interacts
{
    public class DroppedParts : DroppedItem
    {
        [SerializeField] private WeaponPartsSO _partsSO;
        private WeaponParts _parts;

        protected override void Awake()
        {
            base.Awake();
            SetParts(_partsSO.GetWeaponPartsClass());
        }

        private void SetParts(WeaponParts parts)
        {
            _parts = parts;
            _partsSO = parts.WeaponPartsSO;
            SetItemSO(_partsSO);
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            EntityWeapon weaponHolder = entity.GetEntityComponent<EntityWeapon>();
            WeaponParts weaponParts = weaponHolder.CurrentWeapon.EquipParts(_partsSO.partsType, _parts);
            if (weaponParts != null) SetParts(weaponParts);
            else Destroy(gameObject);
        }
    }
}
