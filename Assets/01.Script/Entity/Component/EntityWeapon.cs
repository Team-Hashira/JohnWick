using Hashira.Core.EventSystem;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityWeapon : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IEntityDisposeComponent
    {
        public Weapon CurrentWeapon
        {
            get => Weapons[WeaponIndex];
            protected set => Weapons[WeaponIndex] = value;
        }

        public int WeaponIndex { get; protected set; } = 0;
        public Weapon[] Weapons { get; protected set; }

        protected Entity _entity;

        [field: SerializeField] public Transform VisualTrm { get; private set; }

        protected float _startYPos;
        protected SpriteRenderer _spriteRenderer;

        public Action<Weapon>[] OnChangedWeaponEvents;
        public Action<Weapon> OnCurrentWeaponChanged;

        public GameEventChannelSO SoundEventChannel;

        public virtual void Initialize(Entity entity)
        {
            _entity = entity;
            _spriteRenderer = VisualTrm.GetComponent<SpriteRenderer>();
            WeaponIndex = 0;
            OnCurrentWeaponChanged += HandleChangedCurrentWeaponChangedEvent;

            _startYPos = transform.localPosition.y;
        }

        public virtual void AfterInit()
        {

        }

        protected virtual void HandleChangedCurrentWeaponChangedEvent(Weapon weapon)
        {
            _spriteRenderer.sprite = weapon?.WeaponSO.itemDefaultSprite;

            if (weapon != null)
            {
                Vector3 position = transform.localPosition;
                position.y = _startYPos + weapon.WeaponSO.GrapOffset.y;
                transform.localPosition = position;
                VisualTrm.localEulerAngles = new Vector3(0, 0, weapon.WeaponSO.GrapRotate);

                Vector3 visualPosition = VisualTrm.localPosition;
                visualPosition.x = weapon.WeaponSO.GrapOffset.x;
                visualPosition.y = 0;

                VisualTrm.localPosition = visualPosition;
            }
        }

        public virtual void Attack(int damage, bool isDown)
        {
            CurrentWeapon?.Attack(damage, isDown);

            var evt = SoundEvents.SoundGeneratedEvent;
            evt.originPosition = transform.position;
            evt.loudness = 10;
            SoundEventChannel.RaiseEvent(evt);
        }

        public virtual void RemoveWeapon(int index)
        {
            Weapons[index]?.UnEquip();
            Weapons[index] = null;

            for (int i = 0; i < Weapons.Length; i++)
            {
                WeaponIndex++;
                if (WeaponIndex >= Weapons.Length) WeaponIndex = 0;
                if (Weapons[WeaponIndex] != null) break;
            }
            OnChangedWeaponEvents[index]?.Invoke(null);
        }

        public virtual Weapon EquipWeapon(Weapon weapon, int index = -1)
        {
            int weaponIndex = 0;
            if (index == -1)
            {
                bool hasNullSlot = false;
                for (int i = 0; i < Weapons.Length; i++)
                {
                    if (Weapons[i] == null)
                    {
                        weaponIndex = i;
                        hasNullSlot = true;
                        break;
                    }
                }
                if (hasNullSlot == false)
                    weaponIndex = WeaponIndex;
            }
            else
                weaponIndex = index >= Weapons.Length ? Weapons.Length - 1 : index;
            Weapon prevGunWeapon = Weapons[weaponIndex];

            Weapons[weaponIndex]?.UnEquip();
            Weapons[weaponIndex] = weapon;
            Weapons[weaponIndex]?.Equip(this);

            OnChangedWeaponEvents[weaponIndex]?.Invoke(weapon);

            return prevGunWeapon;
        }

        public void WeaponChange(int index)
        {
            if (index >= Weapons.Length)
                index = Weapons.Length - 1;

            WeaponIndex = index;
            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }

        public void WeaponSwap()
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                WeaponIndex++;
                if (WeaponIndex >= Weapons.Length) WeaponIndex = 0;
                if (CurrentWeapon != null) break;
            }

            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }

        protected virtual void Update()
        {
            CurrentWeapon?.WeaponUpdate();
        }

        public virtual void Dispose()
        {
            OnCurrentWeaponChanged -= HandleChangedCurrentWeaponChangedEvent;
        }
    }
}
