using Crogen.CrogenPooling;
using Hashira.Entities;
using Hashira.Players;
using Hashira.Projectiles;
using System;
using System.Collections.Generic;
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
        private float _lastAttackTime;
        private int _burstBulletCount = 1;

        private List<ProjectileModifier> _projectileModifiers;

        private void Awake()
        {
            _player = GameManager.Instance.Player;

            _input.OnAttackEvent += HandleAttackEvent;
            _lastAttackTime = Time.time;
        }

        private void HandleAttackEvent(bool isDown)
        {
            float angle = 45f;
            if (isDown && _lastAttackTime + _attackDelay < Time.time)
            {
                _lastAttackTime = Time.time;
                for (int i = 0; i < _burstBulletCount; i++)
                {
                    Vector2 targetPos = Camera.main.ScreenToWorldPoint(_input.MousePosition) - transform.position;
                    targetPos = Quaternion.Euler(0, 0, -angle / 2 + angle * (i + 0.5f) / _burstBulletCount) * targetPos;
                    Bullet bullet = gameObject.Pop(ProjectilePoolType.Bullet, transform.position, Quaternion.identity) as Bullet;
                    bullet.Init(_whatIsTarget, targetPos, 100f, _playerStat.StatDictionary["AttackPower"].IntValue, 0, _player.transform);
                }
            }
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
