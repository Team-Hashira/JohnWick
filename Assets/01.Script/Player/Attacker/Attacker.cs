using Crogen.CrogenPooling;
using Hashira.Entities;
using Hashira.Items.Modules;
using Hashira.Players;
using Hashira.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Hashira
{
    public class Attacker : MonoBehaviour
    {
        private Player _player;
        private EntityStat _playerStat;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private InputReaderSO _input;
        [SerializeField] private float _followSpeed = 4f;
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private float _attackDelay;
        [SerializeField] private AnimationCurve _damageOverDistance;
        private float _lastAttackTime;
        private int _burstBulletCount = 1;
        private ProjectilePoolType _projectilePoolType = ProjectilePoolType.Bullet;

        public event Action OnProjectileCreateReadyEvent;
        public event Action<List<Projectile>> OnProjectileCreateEvent;

        private List<Module> _moduleList = new List<Module>();

        private ProjectileModifier _currentMainModifier;

        private void Awake()
        {
            _player = GameManager.Instance.Player;

            _input.OnAttackEvent += HandleAttackEvent;
            _lastAttackTime = Time.time;
        }

        private void Start()
        {
            _playerStat = _player.GetEntityComponent<EntityStat>();
        }

        public void AddBurstBullets()
            => _burstBulletCount++;
        public void RemoveBurstBullets()
            => _burstBulletCount--;
        public void AddModule(Module module)
        {
            _moduleList.Add(module);
        }
        public void RemoveModule(Module module)
        {
            _moduleList.Remove(module);
        }

        public bool TrySetMainProjectileModifier(ProjectileModifier currentMainModifier)
        {
            if (_currentMainModifier != null) return false;
            _currentMainModifier = currentMainModifier;
            return true;
        }

        private void HandleAttackEvent(bool isDown)
        {
            float angle = _burstBulletCount * 5;
            if (isDown && _lastAttackTime + _attackDelay < Time.time)
            {
                OnProjectileCreateReadyEvent?.Invoke();
                _lastAttackTime = Time.time;
                List<Projectile> createdProjectileList = new List<Projectile>();
                for (int i = 0; i < _burstBulletCount; i++)
                {
                    Vector2 targetPos = Camera.main.ScreenToWorldPoint(_input.MousePosition) - transform.position;
                    targetPos = Quaternion.Euler(0, 0, -angle / 2 + angle * (i + 0.5f) / _burstBulletCount) * targetPos;
                    int damage = _playerStat.StatDictionary["AttackPower"].IntValue;
                    Projectile bullet = gameObject.Pop(_projectilePoolType, transform.position, Quaternion.identity) as Projectile;
                    List<ProjectileModifier> projectileModifiers = new List<ProjectileModifier>();
                    _moduleList.ForEach(module => projectileModifiers.AddRange(module.ProjectileModifierList));
                    bullet.Init(_whatIsTarget, targetPos, 100f, damage, 0, _player.transform, projectileModifiers, _damageOverDistance);

                    createdProjectileList.Add(bullet);
                }
                OnProjectileCreateEvent?.Invoke(createdProjectileList);
            }
        }

        public ProjectilePoolType SetProjectile(ProjectilePoolType projectilePoolType)
        {
            ProjectilePoolType prev = _projectilePoolType;
            _projectilePoolType = projectilePoolType;
            return prev;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _player.transform.position + _player.VisualTrm.right * _offset.x + Vector3.up * _offset.y, Time.deltaTime * _followSpeed);
        }

        private void OnDestroy()
        {
            _input.OnAttackEvent -= HandleAttackEvent;
        }
    }
}
