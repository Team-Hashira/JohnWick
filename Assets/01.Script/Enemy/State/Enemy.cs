using Crogen.CrogenPooling;
using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.LatestFSM;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Enemies
{
    public class Enemy : Entity
    {
        protected EntityRenderer _entityRenderer;
        protected EntityHealth _entityHealth;
        protected EntityStat _entityStat;

        [field: SerializeField]
        public GameEventChannelSO SoundEventChannel { get; private set; }

        //Test
        [SerializeField] private EffectPoolType _dieEffect;
        [SerializeField]
        private LayerMask _whatIsPlayer;

        private Player _player;

        protected override void Awake()
        {
            base.Awake();

            _player = GameManager.Instance.Player;
            var partCollider = GetEntityComponent<EntityPartCollider>();
            partCollider.OnPartCollisionHitEvent += HandlePartsCollisionHitEvent;
            _entityHealth.OnDieEvent += HandleDieEvent;
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

        private void HandlePartsCollisionHitEvent(EEntityPartType parts, RaycastHit2D raycastHit, Transform attackerTrm)
        {
            switch (parts)
            {
                case EEntityPartType.Head:
                    _entityRenderer.Blink(0.2f, DG.Tweening.Ease.InCirc);
                    break;
                case EEntityPartType.Legs:
                    _entityStat.StatDictionary["Speed"].AddModify("LegFracture", -50f, Core.StatSystem.EModifyMode.Percnet);
                    break;
            }
        }

        public Player DetectPlayer()
        {
            Collider2D coll = Physics2D.OverlapCircle(transform.position, 5, _whatIsPlayer);
            if (coll == null)
                return null;
            if (Mathf.Sign(coll.transform.position.x - transform.position.x) != _entityRenderer.FacingDirection)
                return null;
            return coll.GetComponent<Player>();
        }
    }
}