using Hashira.Core.StatSystem;
using Hashira.Entities;
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

        protected StatElement _damageStat;

        public Transform _weaponHolder;

        private List<Weapon> _weaponList;
        private int _weaponIndex;
        public Weapon CurrentWeapon { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            _stateMachine = new StateMachine(this, "Hashira.Players.");

            InputReader.OnAttackEvent += HandleAttackEvent;
            InputReader.OnMeleeAttackEvent += HandleMeleeAttackEvent;
            InputReader.OnDashEvent += HandleDashEvent;
            InputReader.OnWeaponSawpEvent += HandleWeaponSawpEvent;

            _weaponList = new List<Weapon>();
            _weaponHolder.GetComponentsInChildren(_weaponList);
            _weaponList.ForEach(weapon => weapon.gameObject.SetActive(false));

            _weaponIndex = -1;
            HandleWeaponSawpEvent();
        }

        private void HandleWeaponSawpEvent()
        {
            if (CurrentWeapon != null) 
                CurrentWeapon.gameObject.SetActive(false);

            _weaponIndex++;
            if (_weaponIndex >= _weaponList.Count) _weaponIndex -= _weaponList.Count;

            CurrentWeapon = _weaponList[_weaponIndex];
            CurrentWeapon.gameObject.SetActive(true);
        }

        private void HandleDashEvent()
        {
            _stateMachine.ChangeState(EPlayerState.Dash);
        }

        private void HandleMeleeAttackEvent()
        {
            CurrentWeapon.MeleeAttack(_damageStat.IntValue);
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            _statCompo = GetEntityComponent<EntityStat>();
            _renderCompo = GetEntityComponent<EntityRenderer>();
            _damageStat = _statCompo.GetElement("AttackPower");
        }

        private void HandleAttackEvent(bool isDown)
        {
            CurrentWeapon.MainAttack(_damageStat.IntValue, isDown);
        }

        protected override void Update()
        {
            base.Update();

            _stateMachine.UpdateMachine();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(InputReader.MousePosition);
            mousePos.z = 0;
            CurrentWeapon.LookTarget(mousePos);

            _renderCompo.LookTarget(mousePos);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputReader.OnAttackEvent -= HandleAttackEvent;
            InputReader.OnMeleeAttackEvent -= HandleMeleeAttackEvent;
            InputReader.OnDashEvent -= HandleDashEvent;
            InputReader.OnWeaponSawpEvent -= HandleWeaponSawpEvent;
        }
    }
}