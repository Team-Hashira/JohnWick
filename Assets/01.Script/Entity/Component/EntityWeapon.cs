using Hashira.Items.Weapons;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityWeapon : MonoBehaviour, IEntityComponent, IEntityDisposeComponent
    {
        public Weapon CurrentWeapon
        {
            get => Weapons[WeaponIndex];
            private set => Weapons[WeaponIndex] = value;
        }

        public int WeaponIndex { get; private set; } = 0;
        public Weapon[] Weapons { get; private set; } = new Weapon[3] { null, null, null };
        public float Facing { get; private set; }

        // Melee Weapon
        public bool IsMeleeWeapon => WeaponIndex == 2; // TODO
        
        [field: SerializeField] public Transform VisualTrm { get; private set; }
        [field: SerializeField] public ParticleSystem CartridgeCaseParticle { get; internal set; }

        public Action<Weapon>[] OnChangedWeaponEvents = new Action<Weapon>[3];

        private SpriteRenderer _spriteRenderer;
        public event Action<Weapon> OnCurrentWeaponChanged;

        private Player _player;

        
        public void Initialize(Entity entity)
        {
            _spriteRenderer = VisualTrm.GetComponent<SpriteRenderer>();

            WeaponIndex = 0;
            OnCurrentWeaponChanged += HandleChangedCurrentWeaponChangedEvent;
            HandleChangedCurrentWeaponChangedEvent(CurrentWeapon);

            _player = entity as Player;
            _player.InputReader.OnReloadEvent += HandleReloadEvent;
        }

        private void HandleReloadEvent()
        {
            if (CurrentWeapon is GunWeapon gun)
                gun?.Reload();
        }

        private void HandleChangedCurrentWeaponChangedEvent(Weapon weapon)
        {
            //����� ���⿡ ���� Visual�� ����
            _spriteRenderer.sprite = weapon?.WeaponSO.itemSprite;
            VisualTrm.gameObject.SetActive(weapon != null);
        }

        public void RemoveWeapon(int index)
        {
            //���� ������ ���⸦ ����
            Weapons[index]?.UnEquip();
            Weapons[index] = null;
            Weapons[index]?.Equip(this);
            
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
                
                Weapons[meleeIndex]?.UnEquip();
                Weapons[meleeIndex] = meleeWeapon;
                Weapons[meleeIndex]?.Equip(this);
                
                OnChangedWeaponEvents[meleeIndex]?.Invoke(meleeWeapon);
                OnCurrentWeaponChanged?.Invoke(Weapons[meleeIndex]);
                
                return prevWeapon;
            }
            
            CurrentWeapon?.UnEquip();
            CurrentWeapon = weapon;
            CurrentWeapon?.Equip(this);
            
            OnChangedWeaponEvents[WeaponIndex]?.Invoke(weapon);
            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);

            //������ ������ �ִ� ���⸦ ��ȯ
            return prevWeapon;
        }

        public void WeaponSwap()
        {
            //���� �ε��� ���ϱ�
            WeaponIndex++;
            if (WeaponIndex >= 2) WeaponIndex = 0;

            OnCurrentWeaponChanged?.Invoke(CurrentWeapon);
        }

        public void Attack(int damage, bool isDown)
            => CurrentWeapon?.Attack(damage, isDown);

        public void LookTarget(Vector3 targetPos)
        {
            Facing = MathF.Sign(targetPos.x - transform.position.x);
            Vector3 dir = targetPos - transform.position;
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg * Facing);
        }


        private void Update()
        {
            CurrentWeapon?.WeaponUpdate();
        }

        public void Dispose()
        {
            _player.InputReader.OnReloadEvent -= HandleReloadEvent;
        }
    }
}
