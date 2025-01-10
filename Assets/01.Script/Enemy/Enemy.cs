using Crogen.CrogenPooling;
using Hashira.Entities;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Enemies
{
    public class Enemy : Entity
    {
        protected EntityRenderer _entityRenderer;
        protected EntityHealth _entityHealth;

        //Test
        [SerializeField] private EffectPoolType _dieEffect;

        private Player _player;

        protected override void Awake()
        {
            base.Awake();

            _player = GameManager.Instance.Player;

            GetEntityComponent<EntityPartCollider>().OnPartCollisionHitEvent += HandlePartsCollisionHitEvent;
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
        }

        protected override void Update()
        {
            base.Update();

            _entityRenderer.LookTarget(_player.transform.position);
        }

        private void HandlePartsCollisionHitEvent(EEntityPartType parts, RaycastHit2D raycastHit, Transform attackerTrm)
        {

            if (parts == EEntityPartType.Head)
            {
                _entityRenderer.Blink(0.2f, DG.Tweening.Ease.InCirc);
            }
        }
    }
}