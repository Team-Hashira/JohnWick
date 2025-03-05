using Hashira.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Projectile : PushLifetime
    {
        [SerializeField] protected bool _canMultipleAttacks;
        [SerializeField] protected BoxDamageCaster2D _boxDamageCaster;
        protected float _speed;
        public int Damage { get; protected set; }

        protected int _penetration;
        protected int _currentPenetration;
        public LayerMask WhatIsTarget { get; protected set; }

        protected List<ProjectileModifier> _projectileModifiers;

        protected List<Collider2D> _penetratedColliderList = new List<Collider2D>();
        public Transform Owner { get; set; }

        protected EAttackType _attackType;
        private Vector3 _spawnPos;

        public event Action<HitInfo> OnHitEvent;

        protected virtual void Awake()
        {
            _boxDamageCaster = GetComponent<BoxDamageCaster2D>();
        }

        protected virtual void FixedUpdate()
        {
            if (_isDead) return;

            for (int i = 0; i < _projectileModifiers.Count; i++)
                _projectileModifiers[i].OnProjectileUpdate();

            Vector3 movement = transform.right * Time.fixedDeltaTime * _speed;
            HitInfo[] hitInfoes = _boxDamageCaster.CastDamage(Damage, movement, transform.right);

            transform.position += movement;

            bool isHit = hitInfoes.Length > 0;

            if (isHit)
            {
                for (int i = 0; i < hitInfoes.Length; i++)
                    OnHited(hitInfoes[i]);

                this.Push();
            }
        }

        public void DamageOverride(int damage)
            => Damage = damage;

        public void SetAttackType(EAttackType eAttackType = EAttackType.Default)
            => _attackType = eAttackType;

        public virtual int CalculateDamage(float damage)
        {
            return Mathf.CeilToInt(damage);
        }

        public int CalculatePenetration(float damage, int penetratedCount)
        {
            if (penetratedCount == 0) return Mathf.CeilToInt(damage);
            return CalculatePenetration(damage * 0.8f, penetratedCount - 1);
        }

        protected virtual void OnHited(HitInfo hitInfo) { }

        public virtual void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner, List<ProjectileModifier> projectileModifiers = null, AnimationCurve damageOverDistance = null)
        {
            Damage = damage;
            _speed = speed;
            _penetration = penetration;
            _currentPenetration = penetration;
            WhatIsTarget = whatIsTarget;
            transform.right = direction;
            _penetratedColliderList = new List<Collider2D>();
            Owner = owner;

            _projectileModifiers = projectileModifiers;
            _projectileModifiers ??= new List<ProjectileModifier>();

            _spawnPos = transform.position;
        }

        #region Pooling
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }
        public virtual void OnPop()
        {
            base.Die();
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
        }
        public virtual void OnPush()
        {
            base.DelayDie();
            _trailRenderer.enabled = false;
            OnHitEvent = null;
        }
        #endregion
    }
}
