using Crogen.CrogenPooling;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Enemies
{
    public abstract class Enemy : Entity
    {
        protected EntityRenderer _entityRenderer;
        protected EntityHealth _entityHealth;
        protected EntityStat _entityStat;

        [field: SerializeField]
        public GameEventChannelSO SoundEventChannel { get; private set; }

        //Test
        [SerializeField] private EffectPoolType _dieEffect;

        [Header("Detect Player Setting")]
        [SerializeField]
        private Transform _eye;
        [field: SerializeField]
        public LayerMask WhatIsPlayer { get; private set; }

        [SerializeField]
        private LayerMask _whatIsGround;


        private StatElement _fovElement;
        private StatElement _sightElement;
        private StatElement _attackRangeElement;

        protected override void Awake()
        {
            base.Awake();

            var partCollider = GetEntityComponent<EntityPartCollider>();
            partCollider.OnPartCollisionHitEvent += HandlePartsCollisionHitEvent;
            _entityHealth.OnDieEvent += HandleDieEvent;

            _fovElement = _entityStat.StatDictionary["FieldOfView"];
            _sightElement = _entityStat.StatDictionary["Sight"];
            _attackRangeElement = _entityStat.StatDictionary["AttackRange"];
        }

        private void HandleDieEvent()
        {
            gameObject.Pop(_dieEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            _entityRenderer = GetEntityComponent<EntityRenderer>();
            _entityHealth = GetEntityComponent<EntityHealth>();
            _entityStat = GetEntityComponent<EntityStat>();
        }

        protected virtual void HandlePartsCollisionHitEvent(EEntityPartType parts, RaycastHit2D raycastHit, Transform attackerTrm)
        {
            switch (parts)
            {
                case EEntityPartType.Head:
                    _entityRenderer.Blink(0.2f, DG.Tweening.Ease.InCirc);
                    break;
                case EEntityPartType.Legs:
                    _entityStat.StatDictionary["Speed"].AddModify("LegFracture", -50f, Core.StatSystem.EModifyMode.Percnet, false);
                    break;
            }
        }

        public virtual Player DetectPlayer()
        {
            Collider2D coll = Physics2D.OverlapCircle(transform.position, _sightElement.Value, WhatIsPlayer);
            if (coll == null)
                return null;
            Vector3 direction = coll.transform.position - _eye.transform.position;
            float distance = direction.magnitude;
            direction.Normalize();
            //if (!Physics2D.Raycast(_eye.transform.position, direction, distance, _whatIsGround))
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (angle < 0)
                    angle += 360f;

                float facingAngle = _entityRenderer.FacingDirection == 1 ? 0 : 180;
                float minAngle = facingAngle - _fovElement.Value;
                float maxAngle = facingAngle + _fovElement.Value;

                if (minAngle <= angle && angle <= maxAngle)
                {
                    return coll.GetComponent<Player>();
                }
            }
            return null;
        }

        public virtual bool IsTargetOnAttackRange(Transform target)
        {
            float distanceSqr = (transform.position - target.position).sqrMagnitude;
            float attackRangeSqr = _attackRangeElement.Value * _attackRangeElement.Value;

            return distanceSqr < attackRangeSqr;
        }
    }
}