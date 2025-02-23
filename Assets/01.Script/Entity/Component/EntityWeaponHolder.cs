using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityWeaponHolder : EntityItemHolder
    {
        public Weapon CurrentWeapon => CurrentItem as Weapon;
        [SerializeField] private GunSO[] _defaultWeapons;

        [field: SerializeField] public PartsRenderer PartsRenderer { get; private set; }
        [field: SerializeField] public ParticleSystem CartridgeCaseParticle { get; internal set; }

        [SerializeField] private LayerMask _whatIsObstacle;

        public float Recoil { get; private set; }
        public bool IsReloading { get; private set; }
        public bool IsStuck { get; private set; } //
        public float Facing { get; private set; }

        private float _currentReloadTime;

        public bool IsSubItemMode;

        public event Action<float> OnReloadEvent;

        private Player _player;

        private Bounds _visualBounds;

        public Action<Weapon>[] OnChangedWeaponEvents;
        public Action<Weapon> OnCurrentWeaponChanged;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);

            Items = new Item[_defaultWeapons.Length];

            OnChangedItemEvents = new Action<Item>[_defaultWeapons.Length];
            OnChangedWeaponEvents = new Action<Weapon>[_defaultWeapons.Length];
            for (int i = 0; i < OnChangedItemEvents.Length; i++)
            {
                int index = i;
                OnChangedItemEvents[index] += item => OnChangedWeaponEvents[index]?.Invoke(item as Weapon);
            }
            OnCurrentItemChanged += item => OnCurrentWeaponChanged?.Invoke(item as Weapon);

            PartsRenderer.Init();

            OnCurrentWeaponChanged += HandleChangedCurrentItem;
        }

        public override void AfterInit()
        {
            base.AfterInit();
            Items = new GunWeapon[_defaultWeapons.Length];
            for (int i = 0; i < _defaultWeapons.Length; i++)
            {
                if (_defaultWeapons[i] == null) continue;
                EquipItem(_defaultWeapons[i].GetItemClass() as GunWeapon, i);
            }
        }

        private void Start()
        {
            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }

        protected override void HandleChangedCurrentItem(Item item)
        {
            base.HandleChangedCurrentItem(item);

            Weapon weapon = item as Weapon;

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

            PartsRenderer.SetGun(weapon as GunWeapon);

            Recoil = 0;
            _currentReloadTime = 0;
            OnReloadEvent?.Invoke(0);
            IsReloading = false;

            if (weapon != null && weapon is GunWeapon gun)
                CartridgeCaseParticle.transform.localPosition = gun.GunSO.cartridgeCaseParticlePoint;



            _visualBounds = _spriteRenderer.bounds;
        }

        public override void RemoveWeapon(int index)
        {
            bool isCurrentWeapon = index == CurrentIndex;
            base.RemoveWeapon(index);

            if (IsSubItemMode == false && isCurrentWeapon)
                OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }

        public override T EquipItem<T>(T Item, int index = -1)
        {
            (Item as GunWeapon)?.SetPartsRenderer(PartsRenderer);

            if (IsSubItemMode == false && (index == CurrentIndex || index == -1))
                OnCurrentWeaponChanged?.Invoke(CurrentWeapon);

            return base.EquipItem(Item, index);
        }

        public virtual void Attack(int damage, bool isDown, LayerMask whatIsTarget)
        {
            if (IsReloading) return;
            CurrentWeapon?.Attack(damage, isDown, whatIsTarget);
            if (isDown)
                _soundGenerator.SoundGenerate(10);
        }

        public void LookTarget(Vector3 targetPos)
        {
            if (IsSubItemMode)
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
            if (CurrentItem != null && CurrentItem is GunWeapon)
            {
                IsReloading = true;
                _currentReloadTime = 1 / CurrentItem.StatDictionary["ReloadSpeed"].Value;
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
                    (CurrentItem as GunWeapon)?.Reload();
                }
            }

            if (Physics2D.OverlapBox(VisualTrm.position, new Vector2(1f, 0.2f), VisualTrm.eulerAngles.z, _whatIsObstacle))
            {
                IsStuck = true;
            }
            else
            {
                IsStuck = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = VisualTrm.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector2.zero, new Vector2(1f, 0.2f));
        }
    }
}
