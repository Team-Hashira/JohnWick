using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityMeleeWeapon : EntityWeapon
    {
        [SerializeField] private MeleeSO[] _defaultWeapons;

        [field: SerializeField] public DamageCaster2D DamageCaster { get; private set; }

        public EntityGunWeapon GunWaepon { get; private set; }

        private float _meleeAttackCooltime = 0.5f;
        private float _lastMeleeAttackTime;


        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);

            OnChangedWeaponEvents = new Action<Weapon>[_defaultWeapons.Length];

            GunWaepon = entity.GetEntityComponent<EntityGunWeapon>();
        }

        public override void AfterInit()
        {
            base.AfterInit();
            Weapons = new MeleeWeapon[_defaultWeapons.Length];
            for (int i = 0; i < _defaultWeapons.Length; i++)
            {
                if (_defaultWeapons[i] == null) continue;
                EquipWeapon(_defaultWeapons[i].GetItemClass() as MeleeWeapon, i);
            }

            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }

        public override void RemoveWeapon(int index)
        {
            base.RemoveWeapon(index);

            if (index == WeaponIndex)
            {
                OnCurrentWeaponChanged?.Invoke(Weapons[index]);

                if (GunWaepon != null)
                    GunWaepon.EquipWeapon(GunWaepon.CurrentWeapon as GunWeapon, GunWaepon.WeaponIndex);
            }
        }

        public override Weapon EquipWeapon(Weapon meleeWeapon, int index = -1)
        {
            if (index == WeaponIndex)
                OnCurrentWeaponChanged?.Invoke(CurrentWeapon);

            return base.EquipWeapon(meleeWeapon, index);
        }

        public override void Attack(int damage, bool isDown)
        {
            if (CurrentWeapon == null) return;
            if (_lastMeleeAttackTime + _meleeAttackCooltime > Time.time) return;

            _lastMeleeAttackTime = Time.time;

            HandleChangedCurrentWeaponChangedEvent(CurrentWeapon);

            if (GunWaepon != null)
                GunWaepon.IsMeleeWeaponMode = true;

            base.Attack(damage, isDown);
        }
    }
}
