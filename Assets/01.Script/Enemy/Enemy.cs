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
        [SerializeField] private Player _player;
        [SerializeField] private Transform _dieEffect;

        protected override void Awake()
        {
            base.Awake();

            GetEntityComponent<EntityPartCollider>().OnPartCollisionHitEvent += HandlePartsCollisionHitEvent;
            _entityHealth.OnDieEvent += HandleDieEvent;
        }

        private void HandleDieEvent()
        {
            Instantiate(_dieEffect, transform.position, Quaternion.identity);
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

        private void HandlePartsCollisionHitEvent(EEntityPartType parts)
        {
            if (parts == EEntityPartType.Head)
            {
                _entityRenderer.Blink(0.2f, DG.Tweening.Ease.InCirc);
            }
        }
    }
}