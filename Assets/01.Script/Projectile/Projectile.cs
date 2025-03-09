using Crogen.CrogenPooling;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Projectile : MonoBehaviour, IPoolingObject
    {
        public bool CanDieSelf { get; set; } = true;
        [SerializeField] protected bool _canMultipleAttacks;
        [SerializeField] protected BoxDamageCaster2D _boxDamageCaster;
        public float Speed { get; private set; }
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
            Vector3 movement = transform.right * Time.fixedDeltaTime * Speed;

            //이것저것 데미지 추가연산
            int damage = CalculateDamage(Damage);
            //관통 뎀감 연산
            int penetrationDamage = CalculatePenetration(CalculateDamage(damage), _penetration - _currentPenetration);
            HitInfo[] hitInfoes = _boxDamageCaster.CastDamage(penetrationDamage, movement, transform.right);

            transform.position += movement;

            bool isHit = hitInfoes.Length > 0;

            if (isHit)
            {
                for (int i = 0; i < hitInfoes.Length; i++)
                    OnHited(hitInfoes[i]);
                if (CanDieSelf)
                    this.Push();
            }
        }

        public void SetDamage(int damage)
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

        public void Redirection(Vector2 direction, float speed = -1f)
        {
            transform.right = direction;
            if (speed != -1f)
                Speed = speed;
        }

        protected virtual void OnHited(HitInfo hitInfo) { }

        public virtual void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner, List<ProjectileModifier> projectileModifiers = null, AnimationCurve damageOverDistance = null)
        {
            Damage = damage;
            Speed = speed;
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
        }
        public virtual void OnPush()
        {
        }
        #endregion
    }
}
