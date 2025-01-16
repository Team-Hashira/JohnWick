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
        [SerializeField] private WeaponPartsSO _weaponSO;

        protected override void Awake()
        {
            base.Awake();
            SetItemSO(_weaponSO);
        }

        protected override void SetItemSO(ItemSO itemSO)
        {
            _weaponSO = itemSO as WeaponPartsSO;
            base.SetItemSO(itemSO);
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            EntityWeapon weaponHolder = entity.GetEntityComponent<EntityWeapon>();
            WeaponPartsSO weaponPartsSO = weaponHolder.CurrentWeapon.EquipParts(_weaponSO.partsType, _weaponSO);
            if (weaponPartsSO != null) SetItemSO(weaponPartsSO);
            else Destroy(gameObject);
        }
    }
}
