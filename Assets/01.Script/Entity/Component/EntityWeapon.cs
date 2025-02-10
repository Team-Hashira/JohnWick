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
            get => Weapons[CurrentIndex];
            protected set => Weapons[CurrentIndex] = value;
        }

        [field: SerializeField] public Transform VisualTrm { get; private set; }

        public int CurrentIndex { get; protected set; } = 0;
        public Weapon[] Weapons { get; protected set; }

        protected float _startYPos;
        protected SpriteRenderer _spriteRenderer;

        protected Entity _entity;
        protected EntityMover _mover;

        public Action<Weapon>[] OnChangedWeaponEvents;
        public Action<Weapon> OnCurrentWeaponChanged;

        public GameEventChannelSO SoundEventChannel;

        public virtual void Initialize(Entity entity)
        {
            _entity = entity;
            _spriteRenderer = VisualTrm.GetComponent<SpriteRenderer>();
            CurrentIndex = 0;
            OnCurrentWeaponChanged += HandleChangedCurrentWeaponChangedEvent;

            _startYPos = transform.localPosition.y;

            _mover = entity.GetEntityComponent<EntityMover>(true);
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

        public virtual void Attack(int damage, bool isDown, LayerMask whatIsTarget)
        {
            CurrentWeapon?.Attack(damage, isDown, whatIsTarget);

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
                CurrentIndex++;
                if (CurrentIndex >= Weapons.Length) CurrentIndex = 0;
                if (Weapons[CurrentIndex] != null) break;
            }
            OnChangedWeaponEvents[index]?.Invoke(null);
        }

        public virtual Weapon EquipWeapon(Weapon weapon, int index)
        {
            int weaponIndex = GetIndex(index);
            if (index == -1) CurrentIndex = weaponIndex;
            Weapon prevGunWeapon = CurrentWeapon;

            CurrentWeapon?.UnEquip();
            CurrentWeapon = weapon;
            CurrentWeapon?.Equip(this);

            OnChangedWeaponEvents[CurrentIndex]?.Invoke(weapon);

            return prevGunWeapon;
        }

        protected int GetIndex(int index)
        {
            if (index == -1)
            {
                for (int i = 0; i < Weapons.Length; i++)
                {
                    if (Weapons[i] == null)
                        return i;
                }
                return CurrentIndex;
            }
            else
                return index >= Weapons.Length ? Weapons.Length - 1 : index;
        }

        public void WeaponChange(int index)
        {
            if (index >= Weapons.Length)
                index = Weapons.Length - 1;

            CurrentIndex = index;
            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }

        public void WeaponSwap()
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                CurrentIndex++;
                if (CurrentIndex >= Weapons.Length) CurrentIndex = 0;
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
