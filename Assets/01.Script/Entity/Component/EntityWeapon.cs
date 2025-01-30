using Hashira.Items.Weapons;
using Hashira.Players;
using System;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Hashira.Entities.Components
{
    public class EntityWeapon : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IEntityDisposeComponent
    {
        [SerializeField] private WeaponSO[] _defaultWeapons;

        public float Recoil { get; private set; }

        public bool IsReloading { get; private set; }
        private float _currentReloadTime;

        public Weapon CurrentWeapon
        {
            get => Weapons[WeaponIndex];
            private set
            {
                if(OldWeaponIndex != WeaponIndex)
                    OldWeaponIndex = WeaponIndex;
                Weapons[WeaponIndex] = value;
            }
        }

        public int WeaponIndex { get; private set; } = 0;
        public Weapon[] Weapons { get; private set; } = new Weapon[3] { null, null, null };
        public float Facing { get; private set; }

        // Melee Weapon
        public bool IsMeleeWeapon => WeaponIndex == 2; // TODO
        
        [field: SerializeField] public Transform VisualTrm { get; private set; }
        [field: SerializeField] public LineRenderer LaserRenderer { get; private set; }
        [field: SerializeField] public ParticleSystem CartridgeCaseParticle { get; internal set; }

        private float _startYPos;
        private SpriteRenderer _spriteRenderer;
        
        public readonly Action<Weapon>[] OnChangedWeaponEvents = new Action<Weapon>[3];
        public event Action<Weapon> OnCurrentWeaponChanged;
        public event Action<float> OnReloadEvent;
        
        private Entity _entity;
        private EntityMover _mover;

        private int _oldWeaponIndex;

        public int OldWeaponIndex
        {
            get => _oldWeaponIndex;
            set
            {
                // ReSharper disable once RedundantCheckBeforeAssignment
                if (_oldWeaponIndex != value)
                    _oldWeaponIndex = value;
            }
        }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _spriteRenderer = VisualTrm.GetComponent<SpriteRenderer>();

            WeaponIndex = 0;
            OnCurrentWeaponChanged += HandleChangedCurrentWeaponChangedEvent;

            if (entity is Player player)
                player.InputReader.OnReloadEvent += HandleReloadEvent;
            _mover = entity.GetEntityComponent<EntityMover>(true);

            _startYPos = transform.localPosition.y;
        }

        public void AfterInit()
        {
            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }

        private void Start()
        {
            foreach (var weaponSO in _defaultWeapons)
                EquipWeapon(weaponSO.GetItemClass() as Weapon);
        }

        private void HandleReloadEvent()
        {
            if (CurrentWeapon != null && CurrentWeapon is GunWeapon)
                Reload(CurrentWeapon.StatDictionary["ReloadSpeed"].Value);
        }

        private void HandleChangedCurrentWeaponChangedEvent(Weapon weapon)
        {
            _spriteRenderer.sprite = weapon?.WeaponSO.itemSprite;
            //VisualTrm.gameObject.SetActive(weapon != null);

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

                if (weapon is GunWeapon gun)
                {
                    GunSO gunSO = gun.GunSO;
                    CartridgeCaseParticle.transform.localPosition = gunSO.cartridgeCaseParticlePoint;
                    visualPosition.y = -gunSO.firePoint.y;
                }
                VisualTrm.localPosition = visualPosition;
            }
        }

        public void RemoveWeapon(int index)
        {
            //���� ������ ���⸦ ����
            Weapons[index]?.UnEquip();
            Weapons[index] = null;
            
            OnChangedWeaponEvents[index]?.Invoke(null);
            OnCurrentWeaponChanged?.Invoke(Weapons[index]);
        }
        
        public Weapon EquipWeapon(Weapon weapon)
        {
            //���� ������ ���⸦ ����
            Weapon prevWeapon = CurrentWeapon;
            if (weapon is MeleeWeapon meleeWeapon)
            {
                int meleeIndex = 2;
                transform.localEulerAngles = Vector3.zero;
				Weapon preMeleeWeapon = Weapons[meleeIndex];
                
                Weapons[meleeIndex]?.UnEquip();
                Weapons[meleeIndex] = meleeWeapon;
                Weapons[meleeIndex]?.Equip(this);
                
                OnChangedWeaponEvents[meleeIndex]?.Invoke(meleeWeapon);
                
                return preMeleeWeapon;
            }
            
            CurrentWeapon?.UnEquip();
            CurrentWeapon = weapon;
            CurrentWeapon?.Equip(this);
            
            OnChangedWeaponEvents[WeaponIndex]?.Invoke(weapon);
            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);

            //������ ������ �ִ� ���⸦ ��ȯ
            return prevWeapon;
        }

        public void WeaponChange(int index)
        {
            if(index > Weapons.Length - 1)
                index = Weapons.Length - 1;
            
            WeaponIndex = index;
            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }
        
        public void WeaponSwap()
        {
            //빈 슬롯에 자동삽입 기능을 추가할 지 안정했기에 일단 주석
            //if (Weapons.Where(x => x != null && x is not MeleeWeapon).ToArray().Length == 1) return;
            WeaponIndex++;
            if (WeaponIndex >= 2) WeaponIndex = 0;

            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }

        public void Attack(int damage, bool isDown, bool isMelee = false)
        {
            if (IsReloading) return;
            if (isMelee)
            {
				if (CurrentWeapon == null) return;
				WeaponIndex = 2;
                OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
            }
            CurrentWeapon?.Attack(damage, isDown);
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

        public void Dispose()
        {
            if (_entity is Player player)
                player.InputReader.OnReloadEvent -= HandleReloadEvent;
        }
    }
}
