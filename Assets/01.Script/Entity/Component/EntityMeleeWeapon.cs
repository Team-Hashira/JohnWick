using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using Hashira.Players;
using System;
using System.Linq;
using UnityEngine;
    
namespace Hashira.Entities.Components
{
    public class EntityMeleeWeapon : EntityWeapon
    {
        [SerializeField] private MeleeSO[] _defaultWeapons;

        public MeleeWeapon CurrentWeapon
        {
            get => Weapons[WeaponIndex];
            private set => Weapons[WeaponIndex] = value;
        }

        public int WeaponIndex { get; private set; } = 0;
        public MeleeWeapon[] Weapons { get; private set; }
        public float Facing { get; private set; }

        // Melee Weapon
        public bool IsMeleeWeapon => WeaponIndex == 2; // TODO

        [field: SerializeField] public Transform VisualTrm { get; private set; }

        private float _startYPos;
        private SpriteRenderer _spriteRenderer;

        public readonly Action<MeleeWeapon>[] OnChangedWeaponEvents = new Action<MeleeWeapon>[3];
        public event Action<MeleeWeapon> OnCurrentWeaponChanged;
        public event Action<float> OnReloadEvent;

        private Entity _entity;
        private EntityMover _mover;
        public EntityGunWeapon GunWaepon { get; private set; }

        [field: SerializeField] public DamageCaster2D DamageCaster { get; private set; }

        public GameEventChannelSO SoundEventChannel;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _entity = entity;
            _spriteRenderer = VisualTrm.GetComponent<SpriteRenderer>();

            WeaponIndex = 0;
            OnCurrentWeaponChanged += HandleChangedCurrentWeaponChangedEvent;

            _mover = entity.GetEntityComponent<EntityMover>(true);
            GunWaepon = entity.GetEntityComponent<EntityGunWeapon>();

            _startYPos = transform.localPosition.y;
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
        }

        private void HandleChangedCurrentWeaponChangedEvent(Weapon weapon)
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

        public void RemoveWeapon(int index)
        {
            Weapons[index]?.UnEquip();
            Weapons[index] = null;

            OnChangedWeaponEvents[index]?.Invoke(null);
            if (IsMeleeWeapon == false && index == WeaponIndex)
                OnCurrentWeaponChanged?.Invoke(Weapons[index]);
        }

        public MeleeWeapon EquipWeapon(MeleeWeapon meleeWeapon, int index = -1)
        {
            transform.localEulerAngles = Vector3.zero;
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
            MeleeWeapon prevMeleeWeapon = Weapons[weaponIndex];

            Weapons[weaponIndex]?.UnEquip();
            Weapons[weaponIndex] = meleeWeapon;
            Weapons[weaponIndex]?.Equip(this);

            OnChangedWeaponEvents[weaponIndex]?.Invoke(meleeWeapon);
            if (weaponIndex == WeaponIndex)
                OnCurrentWeaponChanged?.Invoke(CurrentWeapon);

            return prevMeleeWeapon;
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

        public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
            Debug.Log("근접공격");
            if (CurrentWeapon == null) return;

            HandleChangedCurrentWeaponChangedEvent(CurrentWeapon);

            if (GunWaepon != null)
                GunWaepon.IsMeleeWeapon = true;

            CurrentWeapon?.Attack(damage, isDown);

            var evt = SoundEvents.SoundGeneratedEvent;
            evt.originPosition = transform.position;
            evt.loudness = 10;
            SoundEventChannel.RaiseEvent(evt);
        }
         
        private void Update()
        {
            CurrentWeapon?.WeaponUpdate();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
