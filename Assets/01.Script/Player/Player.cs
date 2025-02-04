using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.Items.Weapons;
using Hashira.LatestFSM;
using UnityEngine;

namespace Hashira.Players
{
    public class Player : Entity
    {
        [field: SerializeField] public InputReaderSO InputReader { get; private set; }
        [field: SerializeField] public Transform VisualTrm { get; private set; }
        [field: SerializeField] public ParticleSystem AfterImageParticle { get; private set; }

        protected EntityStateMachine _stateMachine;
        protected EntityRenderer _renderCompo;
        protected EntityStat _statCompo;
        protected EntityWeapon _weaponHolderCompo;
        protected EntityInteractor _interactor;
        protected PlayerMover _playerMover;

        private Weapon CurrentWeapon => _weaponHolderCompo.CurrentWeapon;

        protected StatElement _damageStat;

        protected override void Awake()
        {
            base.Awake();

            InputReader.OnDashEvent += HandleDashEvent;
            InputReader.OnInteractEvent += HandleInteractEvent;

            InputReader.OnAttackEvent += HandleAttackEvent;
            InputReader.OnMeleeAttackEvent += HandleMeleeAttackEvent;
            InputReader.OnWeaponSwapEvent += HandleWeaponSwapEvent;
        }

        #region Handles

        private void HandleInteractEvent(bool isDown)
        {
            _interactor.Interact(isDown);
        }

        private void HandleDashEvent()
        {
            if (_playerMover.CanDash == false) return;
			if (_stateMachine.CurrentStateName != "Rolling")
            {
			    _playerMover.OnDash();
				_stateMachine.ChangeState("Rolling");
            }
        }

        private void HandleMeleeAttackEvent()
        {
            _weaponHolderCompo?.Attack(_damageStat.IntValue, true, true);
        }

        private void HandleAttackEvent(bool isDown)
        {
            _weaponHolderCompo?.Attack(_damageStat.IntValue, isDown);
        }

        private void HandleWeaponSwapEvent()
        {
            _weaponHolderCompo.WeaponSwap();
        }

        #endregion

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

			_playerMover = GetEntityComponent<PlayerMover>();
			_statCompo = GetEntityComponent<EntityStat>();
            _renderCompo = GetEntityComponent<EntityRenderer>();
            _weaponHolderCompo = GetEntityComponent<EntityWeapon>();
            _interactor = GetEntityComponent<EntityInteractor>();
            _stateMachine = GetEntityComponent<EntityStateMachine>();
            _damageStat = _statCompo.StatDictionary["AttackPower"];
        }

        protected override void Update()
        {
            base.Update();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(InputReader.MousePosition);
            mousePos.z = 0;
            _weaponHolderCompo.LookTarget(mousePos);

            _renderCompo.SetUsualFacingTarget(mousePos);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputReader.OnDashEvent -= HandleDashEvent;
            InputReader.OnInteractEvent -= HandleInteractEvent;

            InputReader.OnAttackEvent -= HandleAttackEvent;
            InputReader.OnMeleeAttackEvent -= HandleMeleeAttackEvent;
            InputReader.OnWeaponSwapEvent -= HandleWeaponSwapEvent;
        }
    }
}