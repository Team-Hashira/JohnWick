using Crogen.CrogenPooling;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Projectile : MonoBehaviour, IPoolingObject
    {
        [SerializeField] protected bool _canMultipleAttacks;
        [SerializeField] protected BoxDamageCaster2D _boxDamageCaster;
        protected float _speed;
        public int Damage { get; protected set; }

        protected int _penetration;
        protected int _currentPenetration;
        public LayerMask WhatIsTarget { get; protected set; }

        protected List<Collider2D> _penetratedColliderList = new List<Collider2D>();
        public Transform Owner { get; set; }

        protected EAttackType _attackType;
        protected AnimationCurve _damageOverDistance;
        private Vector3 _spawnPos;

        public event Action<RaycastHit2D, IDamageable> OnHitEvent;

        protected virtual void Awake()
        {
            _boxDamageCaster = GetComponent<BoxDamageCaster2D>();
        }

        protected virtual void FixedUpdate()
        {
            Vector3 movement = transform.right * Time.fixedDeltaTime * _speed;
            transform.position += movement;

            HitInfo[] hitInfoes = _boxDamageCaster.CastDamage(Damage, transform.position, transform.right);

            bool isHit = hitInfoes.Length > 0;

            if (isHit)
            {
                for (int i = 0; i < hitInfoes.Length; i++)
                    OnHited(hitInfoes[i]);

                this.Push();
                return;
            }

            float spawnPosToCurPosDis = Vector3.Distance(_spawnPos, transform.position);
            float time = _damageOverDistance.keys[^1].time;
            bool isDie = _damageOverDistance.keys.Length != 0 && time < spawnPosToCurPosDis;

            if (isDie)
                this.Push();
        }

        public void SetDamage(int damage)
            => Damage = damage;
        public void SetAttackType(EAttackType eAttackType = EAttackType.Default)
            => _attackType = eAttackType;
        public virtual int CalculateDamage(float damage)
        {
            float finalDamage = damage * _damageOverDistance.Evaluate(Vector3.Distance(_spawnPos, transform.position));
            return Mathf.CeilToInt(finalDamage);
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
            _damageOverDistance = damageOverDistance;
            if (_damageOverDistance == null)
            {
                _damageOverDistance = new AnimationCurve();
                _damageOverDistance.AddKey(0, 1);
                _damageOverDistance.AddKey(10000, 1);
            }

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
