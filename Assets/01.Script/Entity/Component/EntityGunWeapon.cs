using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityGunWeapon : EntityWeapon
    {
        [SerializeField] private GunSO[] _defaultWeapons;

        [field: SerializeField] public PartsRenderer PartsRenderer { get; private set; }
        [field: SerializeField] public ParticleSystem CartridgeCaseParticle { get; internal set; }

        public float Recoil { get; private set; }
        public bool IsReloading { get; private set; }
        public float Facing { get; private set; }

        private float _currentReloadTime;

        public bool IsMeleeWeaponMode;

        public event Action<float> OnReloadEvent;

        private Player _player;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);

            OnChangedWeaponEvents = new Action<Weapon>[_defaultWeapons.Length];

            PartsRenderer.Init();

            OnCurrentWeaponChanged += HandleChangedCurrentWeaponChangedEvent;
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

        protected override void HandleChangedCurrentWeaponChangedEvent(Weapon weapon)
        {
            base.HandleChangedCurrentWeaponChangedEvent(weapon);

            PartsRenderer.SetGun(weapon as GunWeapon);

            Recoil = 0;
            _currentReloadTime = 0;
            OnReloadEvent?.Invoke(0);
            IsReloading = false;

            if (weapon != null && weapon is GunWeapon gun)
                CartridgeCaseParticle.transform.localPosition = gun.GunSO.cartridgeCaseParticlePoint;
        }

        public override void RemoveWeapon(int index)
        {
            base.RemoveWeapon(index);

            if (IsMeleeWeaponMode == false && index == CurrentIndex)
                OnCurrentWeaponChanged?.Invoke(Weapons[index]);
        }

        public override Weapon EquipWeapon(Weapon weapon, int index = -1)
        {
            (weapon as GunWeapon)?.SetPartsRenderer(PartsRenderer);

            if (IsMeleeWeaponMode == false && (index == CurrentIndex || index == -1))
                OnCurrentWeaponChanged?.Invoke(weapon);

            return base.EquipWeapon(weapon, index);
        }

        public override void Attack(int damage, bool isDown, LayerMask whatIsTarget)
        {
            if (IsReloading) return;
            base.Attack(damage, isDown, whatIsTarget);
        }

        public void LookTarget(Vector3 targetPos)
        {
            if (IsMeleeWeaponMode)
                return;

            Facing = Mathf.Sign(targetPos.x - transform.position.x);
            Vector3 dir = targetPos - transform.position;
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg * Facing);
        }

        public void ApplyRecoil(float value)
        {
            Recoil = Mathf.Clamp(Recoil + value * 0.1f, 0, 10f);
        }

        public void Reload()
        {
            if (IsReloading) return;
            if (CurrentWeapon != null && CurrentWeapon is GunWeapon)
            {
                IsReloading = true;
                _currentReloadTime = CurrentWeapon.StatDictionary["ReloadSpeed"].Value;
            }
        }

        protected override void Update()
        {
            base.Update();

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
    }
}
