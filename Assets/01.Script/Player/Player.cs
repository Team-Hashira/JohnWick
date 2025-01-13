using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Weapons;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Players
{
    public enum EPlayerState
    {
        Idle,
        Walk,
        Dash,
        Crouch,
        Air
    }

    public class Player : Entity
    {
        [field: SerializeField] public InputReaderSO InputReader { get; private set; }
        [field: SerializeField] public Transform VisualTrm { get; private set; }

        protected StateMachine _stateMachine;

        protected EntityRenderer _renderCompo;
        protected EntityStat _statCompo;
        protected EntityWeaponHolder _weaponHolderCompo;
        protected EntityInteractor _interactor;

        private Weapon CurrentWeapon => _weaponHolderCompo.CurrentWeapon;

        protected StatElement _damageStat;

        protected override void Awake()
        {
            base.Awake();

            _stateMachine = new StateMachine(this, "Hashira.Players.");

            InputReader.OnDashEvent += HandleDashEvent;
            InputReader.OnInteractEvent += HandleInteractEvent;

            InputReader.OnAttackEvent += HandleAttackEvent;
            InputReader.OnMeleeAttackEvent += HandleMeleeAttackEvent;
            InputReader.OnReloadEvent += HandleReloadEvent;
            InputReader.OnWeaponSawpEvent += HandleWeaponSawpEvent;
        }

        #region Handles

        private void HandleInteractEvent()
        {
            _interactor.Interact();
        }

        private void HandleDashEvent()
        {
            _stateMachine.ChangeState(EPlayerState.Dash);
        }

        private void HandleMeleeAttackEvent()
        {
            CurrentWeapon?.MeleeAttack(_damageStat.IntValue);
        }

        private void HandleAttackEvent(bool isDown)
        {
            CurrentWeapon?.MainAttack(_damageStat.IntValue, isDown);
        }

        private void HandleReloadEvent()
        {
            if (CurrentWeapon != null &&CurrentWeapon is Gun gun)
                gun.Reload();
        }

        private void HandleWeaponSawpEvent()
        {
            _weaponHolderCompo.WeaponSawp();
        }

        #endregion

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            _statCompo = GetEntityComponent<EntityStat>();
            _renderCompo = GetEntityComponent<EntityRenderer>();
            _weaponHolderCompo = GetEntityComponent<EntityWeaponHolder>();
            _interactor = GetEntityComponent<EntityInteractor>();
            _damageStat = _statCompo.GetElement("AttackPower");
        }

        protected override void Update()
        {
            base.Update();

            _stateMachine.UpdateMachine();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(InputReader.MousePosition);
            mousePos.z = 0;
            CurrentWeapon?.LookTarget(mousePos);

            _renderCompo.LookTarget(mousePos);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputReader.OnDashEvent -= HandleDashEvent;
            InputReader.OnInteractEvent -= HandleInteractEvent;

            InputReader.OnAttackEvent -= HandleAttackEvent;
            InputReader.OnMeleeAttackEvent -= HandleMeleeAttackEvent;
            InputReader.OnWeaponSawpEvent -= HandleWeaponSawpEvent;
            InputReader.OnReloadEvent -= HandleReloadEvent;
        }
    }
}