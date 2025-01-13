using Hashira.Entities.Components;
using Hashira.Weapons;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class DroppedWeapon : KeyInteractObject
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private WeaponSO _weaponSO;

        protected override void Awake()
        {
            base.Awake();
            Init(_weaponSO);
        }

        public void Init(WeaponSO weaponSO)
        {
            _weaponSO = weaponSO;
            _spriteRenderer.sprite = _weaponSO.weaponSprite;
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            EntityWeaponHolder weaponHolder = entity.GetEntityComponent<EntityWeaponHolder>();
            WeaponSO weaponSO = weaponHolder.EquipWeapon(_weaponSO);
            if (weaponSO != null)
                Init(weaponSO);
            else
                Destroy(gameObject);
        }
    }
}
