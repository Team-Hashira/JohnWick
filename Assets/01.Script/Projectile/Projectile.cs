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
        private Vector3 _spawnPos;

        public event Action<HitInfo> OnHitEvent;

        protected virtual void Awake()
        {
            _boxDamageCaster = GetComponent<BoxDamageCaster2D>();
        }

        protected virtual void FixedUpdate()
        {
            Vector3 movement = transform.right * Time.fixedDeltaTime * _speed;
            HitInfo[] hitInfoes = _boxDamageCaster.CastDamage(Damage, movement, transform.right);


            bool isHit = hitInfoes.Length > 0;

            if (isHit)
            {
                if (hitInfoes.Length > 0) Debug.Log(hitInfoes[0].raycastHit.transform);
                for (int i = 0; i < hitInfoes.Length; i++)
                    OnHited(hitInfoes[i]);
                transform.position += transform.right * hitInfoes[0].raycastHit.distance;

                this.Push();
            }
            else
                transform.position += movement;
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
