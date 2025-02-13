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

        public bool IsCharged { get; private set; }

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

            if (index == CurrentIndex)
            {
                OnCurrentWeaponChanged?.Invoke(Weapons[index]);

                if (GunWaepon != null)
                    GunWaepon.EquipWeapon(GunWaepon.CurrentWeapon as GunWeapon, GunWaepon.CurrentIndex);
            }
        }

        public override Weapon EquipWeapon(Weapon weapon, int index = -1)
        {
            if (GunWaepon == null && (index == CurrentIndex || index == -1))
                OnCurrentWeaponChanged?.Invoke(weapon);

            return base.EquipWeapon(weapon, index);
        }

        public override void Attack(int damage, bool isDown, LayerMask whatIsTarget)
        {
            if (CurrentWeapon == null) return;
            if (_lastMeleeAttackTime + _meleeAttackCooltime > Time.time) return;

            _lastMeleeAttackTime = Time.time;

            HandleChangedCurrentWeaponChangedEvent(CurrentWeapon);

            if (GunWaepon != null)
                GunWaepon.IsMeleeWeaponMode = true;

            base.Attack(damage, isDown, whatIsTarget);
        }

        public void ChargeAttack(int damage, bool isDown, LayerMask whatIsTarget)
        {
            if (CurrentWeapon == null) return;

            HandleChangedCurrentWeaponChangedEvent(CurrentWeapon);

            if (GunWaepon != null)
                GunWaepon.IsMeleeWeaponMode = true;

            IsCharged = true;
            base.Attack(damage, isDown, whatIsTarget);
            IsCharged = false;
        }
    }
}
