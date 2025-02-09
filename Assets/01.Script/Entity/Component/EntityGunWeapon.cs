using Hashira.Items.Weapons;
using Hashira.Players;
using Hashira.Core;
using System;
using UnityEngine;
using Hashira.Core.EventSystem;
using Hashira.Items.PartsSystem;
using System.Linq;
using UnityEngine.UI;

namespace Hashira.Entities.Components
{
    public class EntityGunWeapon : EntityWeapon
    {
        [SerializeField] private GunSO[] _defaultWeapons;

        public float Recoil { get; private set; }

        public bool IsReloading { get; private set; }
        private float _currentReloadTime;

        public GunWeapon CurrentWeapon
        {
            get => Weapons[WeaponIndex];
            private set => Weapons[WeaponIndex] = value;
        }

        public int WeaponIndex { get; private set; } = 0;
        public GunWeapon[] Weapons { get; private set; } = new GunWeapon[3] { null, null, null };
        public float Facing { get; private set; }

        // Melee Weapon
        public bool IsMeleeWeapon; // TODO

        [field: SerializeField] public Transform VisualTrm { get; private set; }
        [field: SerializeField] public PartsRenderer PartsRenderer { get; private set; }
        [field: SerializeField] public ParticleSystem CartridgeCaseParticle { get; internal set; }

        private float _startYPos;
        private SpriteRenderer _spriteRenderer;

        public Action<Weapon>[] OnChangedWeaponEvents {  get; private set; }
        public event Action<Weapon> OnCurrentWeaponChanged;
        public event Action<float> OnReloadEvent;

        private Entity _entity;
        private EntityMover _mover;

        [field:SerializeField] public DamageCaster2D DamageCaster { get; private set; }


        public GameEventChannelSO SoundEventChannel;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _entity = entity;
            _spriteRenderer = VisualTrm.GetComponent<SpriteRenderer>();

            OnChangedWeaponEvents = new Action<Weapon>[_defaultWeapons.Length];

            PartsRenderer.Init();

            WeaponIndex = 0;
            OnCurrentWeaponChanged += HandleChangedCurrentWeaponChangedEvent;

            if (entity is Player player)
                player.InputReader.OnReloadEvent += HandleReloadEvent;
            _mover = entity.GetEntityComponent<EntityMover>(true);

            _startYPos = transform.localPosition.y;
        }

        public override void AfterInit()
        {
            base.AfterInit();
            Weapons = new GunWeapon[_defaultWeapons.Length];
            for (int i = 0; i < _defaultWeapons.Length; i++)
            {
                if (_defaultWeapons[i] == null) continue;
                EquipWeapon(_defaultWeapons[i].GetItemClass() as GunWeapon, i);
            }

        }

        private void Start()
        {
            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }

        private void HandleReloadEvent()
        {
            if (CurrentWeapon != null && CurrentWeapon is GunWeapon)
                Reload(CurrentWeapon.StatDictionary["ReloadSpeed"].Value);
        }

        private void HandleChangedCurrentWeaponChangedEvent(Weapon weapon)
        {
            _spriteRenderer.sprite = weapon?.WeaponSO.itemDefaultSprite;

            PartsRenderer.SetGun(weapon as GunWeapon);

            Recoil = 0;
            _currentReloadTime = 0;
            OnReloadEvent?.Invoke(0);
            IsReloading = false;

            if (weapon != null)
            {
                Vector3 position = transform.localPosition;
                position.y = _startYPos + weapon.WeaponSO.GrapOffset.y;
                transform.localPosition = position;
                VisualTrm.localEulerAngles = new Vector3(0, 0, weapon.WeaponSO.GrapRotate);

                Vector3 visualPosition = VisualTrm.localPosition;
                visualPosition.x = weapon.WeaponSO.GrapOffset.x;
                visualPosition.y = 0;

                if (weapon is GunWeapon gun)
                {
                    GunSO gunSO = gun.GunSO;
                    CartridgeCaseParticle.transform.localPosition = gunSO.cartridgeCaseParticlePoint;
                }

                VisualTrm.localPosition = visualPosition;
            }
        }

        public void RemoveWeapon(int index)
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
            if (IsMeleeWeapon == false && index == WeaponIndex)
                OnCurrentWeaponChanged?.Invoke(Weapons[index]);
        }

        public GunWeapon EquipWeapon(GunWeapon gunWeapon, int index = -1)
        {
            int weaponIndex = 0;
            if (index == -1)
            {
                bool hasNullSlot = false;
                for (int i = 0; i < Weapons.Length; i++)
                {
                    if (Weapons[i] == null)
                    {
                        Debug.Log(i + " " + Weapons[i]);
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
            GunWeapon prevGunWeapon = Weapons[weaponIndex];

            gunWeapon?.SetPartsRenderer(PartsRenderer);

            Weapons[weaponIndex]?.UnEquip();
            Weapons[weaponIndex] = gunWeapon;
            Weapons[weaponIndex]?.Equip(this);

            OnChangedWeaponEvents[weaponIndex]?.Invoke(gunWeapon);
            if (weaponIndex == WeaponIndex)
                OnCurrentWeaponChanged?.Invoke(CurrentWeapon);

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

        public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
            if (IsReloading) return;

            CurrentWeapon?.Attack(damage, isDown);

            var evt = SoundEvents.SoundGeneratedEvent;
            evt.originPosition = transform.position;
            evt.loudness = 10;
            SoundEventChannel.RaiseEvent(evt);
        }

        public void LookTarget(Vector3 targetPos)
        {
            if (IsMeleeWeapon)
                return;

            Facing = Mathf.Sign(targetPos.x - transform.position.x);
            Vector3 dir = targetPos - transform.position;
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg * Facing);
        }

        public void ApplyRecoil(float value)
        {
            Recoil = Mathf.Clamp(Recoil + value * 0.1f, 0, 10f);
        }

        public void Reload(float time)
        {
            if (IsReloading) return;
            IsReloading = true;
            _currentReloadTime = time;
        }

        private void Update()
        {
            CurrentWeapon?.WeaponUpdate();

            int defaultRecoil = (_mover.Velocity.x == 0 && _mover.IsGrounded) ? 0 : 1;
            if (Recoil > defaultRecoil)
                Recoil -= Time.deltaTime * 10f;
            if (Recoil < defaultRecoil)
                Recoil = defaultRecoil;

            if (_currentReloadTime > 0)
            {
                _currentReloadTime -= Time.deltaTime;
                OnReloadEvent?.Invoke(_currentReloadTime);
                if (_currentReloadTime < 0)
                {
                    _currentReloadTime = 0;
                    IsReloading = false;
                    OnReloadEvent?.Invoke(_currentReloadTime);
                    (CurrentWeapon as GunWeapon)?.Reload();
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_entity is Player player)
                player.InputReader.OnReloadEvent -= HandleReloadEvent;
        }
    }
}
