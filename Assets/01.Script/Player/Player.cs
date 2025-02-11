using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.Items.Weapons;
using System;
using UnityEditor.Rendering;
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
        protected EntityGunWeapon _weaponGunHolderCompo;
        protected EntityMeleeWeapon _weaponMeleeHolderCompo;
        protected EntityInteractor _interactor;
        protected PlayerMover _playerMover;

        private Weapon CurrentWeapon => _weaponGunHolderCompo.CurrentWeapon;

        protected StatElement _damageStat;

        [Header("=====Stamina setting=====")]
        [field: SerializeField] public float MaxStamina { get; private set; }
        [SerializeField] private float _staminaRecoverySpeed;
        [SerializeField] private float _staminaRecoveryDelay;
        private float _lastStaminaUsedTime;
        private float _currentStamina;
        public event Action<float, float> OnStaminaChangedEvent;

        [Header("=====Layer setting=====")]
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private LayerMask _whatIsObstacle;

        private float _slowTimeScale = 0.1f;
        private float _chargingParryingStartDelay = 0.15f;
        private float _lastRightClickTime;
        private bool _isRightMousePress;
        private bool _isChargingParrying;

        protected override void Awake()
        {
            base.Awake();

            InputReader.OnDashEvent += HandleDashEvent;
            InputReader.OnInteractEvent += HandleInteractEvent;

            InputReader.OnReloadEvent += _weaponGunHolderCompo.Reload;
            InputReader.OnAttackEvent += HandleAttackEvent;
            InputReader.OnMeleeAttackEvent += HandleMeleeAttackEvent;
            InputReader.OnWeaponSwapEvent += HandleWeaponSwapEvent;
        }

        private void Start()
        {
            _currentStamina = MaxStamina;
            OnStaminaChangedEvent?.Invoke(_currentStamina, _currentStamina);
        }

        #region Handles

        private void HandleInteractEvent(bool isDown)
        {
            _interactor.Interact(isDown);
        }

        private void HandleDashEvent()
        {
            if (_playerMover.CanRolling == false) return;
            if (_stateMachine.CurrentStateName != "Rolling")
            {
                if (TryUseStamina(20))
                {
                    _playerMover.OnDash();
                    _stateMachine.ChangeState("Rolling");
                }
                else
                {
                    Debug.Log("스테미나 부족!");
                }
            }
        }

        private void HandleMeleeAttackEvent(bool isDown)
        {
            _isRightMousePress = isDown;
            if (isDown)
            {
                _lastRightClickTime = Time.time;
            }
            else
            {
                if (_isChargingParrying)
                {
                    TimeManager.UndoTimeScale();
                    _isChargingParrying = false;
                    _weaponMeleeHolderCompo?.ChargeAttack(_damageStat.IntValue, true, _whatIsTarget | _whatIsObstacle);
                }
                else
                {
                    _weaponMeleeHolderCompo?.Attack(_damageStat.IntValue, true, _whatIsTarget | _whatIsObstacle);
                }
            }
        }

        private void HandleAttackEvent(bool isDown)
        {
            _weaponGunHolderCompo?.Attack(_damageStat.IntValue, isDown, _whatIsTarget | _whatIsObstacle);
        }

        private void HandleWeaponSwapEvent()
        {
            _weaponGunHolderCompo.WeaponSwap();
        }

        #endregion

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            _playerMover = GetEntityComponent<PlayerMover>();
            _statCompo = GetEntityComponent<EntityStat>();
            _renderCompo = GetEntityComponent<EntityRenderer>();
            _weaponGunHolderCompo = GetEntityComponent<EntityGunWeapon>();
            _weaponMeleeHolderCompo = GetEntityComponent<EntityMeleeWeapon>();
            _interactor = GetEntityComponent<EntityInteractor>();
            _stateMachine = GetEntityComponent<EntityStateMachine>();
            _damageStat = _statCompo.StatDictionary["AttackPower"];
        }

        protected override void AfterIntiialize()
        {
            base.AfterIntiialize();
        }
        
        public bool TryUseStamina(float usedValue)
        {
            if (IsCanUseStamina(usedValue) == false) return false;
            float prevStamina = _currentStamina;
            _currentStamina -= usedValue;
            _lastStaminaUsedTime = Time.time;
            OnStaminaChangedEvent?.Invoke(prevStamina, _currentStamina);
            return true;
        }
        public bool IsCanUseStamina(float usedValue)
            => _currentStamina >= usedValue;

        protected override void Update()
        {
            base.Update();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(InputReader.MousePosition);
            mousePos.z = 0;
            _weaponGunHolderCompo.LookTarget(mousePos);

            _renderCompo.SetUsualFacingTarget(mousePos);


            if (_currentStamina < MaxStamina && _lastStaminaUsedTime + _staminaRecoveryDelay < Time.time)
            {
                float prevStamina = _currentStamina;
                _currentStamina += Time.deltaTime * _staminaRecoverySpeed;
                if (_currentStamina > MaxStamina)
                {
                    _currentStamina = MaxStamina;
                }
                OnStaminaChangedEvent?.Invoke(prevStamina, _currentStamina);
            }


            if (_isRightMousePress && _isChargingParrying  == false && 
                _lastRightClickTime + _chargingParryingStartDelay < Time.time)
            {
                _isChargingParrying = true;
                TimeManager.SetTimeScale(_slowTimeScale);
            }
            if (_isChargingParrying)
            {
                //차징중...
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputReader.OnDashEvent -= HandleDashEvent;
            InputReader.OnInteractEvent -= HandleInteractEvent;

            InputReader.OnReloadEvent -= _weaponGunHolderCompo.Reload;
            InputReader.OnAttackEvent -= HandleAttackEvent;
            InputReader.OnMeleeAttackEvent -= HandleMeleeAttackEvent;
            InputReader.OnWeaponSwapEvent -= HandleWeaponSwapEvent;
        }
    }
}