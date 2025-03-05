using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.TargetPoint;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Hashira.Players
{
    public class Player : Entity
    {
        [field: SerializeField] public InputReaderSO InputReader { get; private set; }
        [field: SerializeField] public Transform VisualTrm { get; private set; }
        [field: SerializeField] public ParticleSystem AfterImageParticle { get; private set; }
        
        
        public Attacker Attacker { get; private set; }

        protected EntityStateMachine _stateMachine;
        protected EntityRenderer _renderCompo;
        protected EntityStat _statCompo;
        protected EntityInteractor _interactor;
        protected PlayerMover _playerMover;

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

        private EntityHealth _entityHealth;

        protected override void Awake()
        {
            base.Awake();

            _entityHealth = GetEntityComponent<EntityHealth>();

            _entityHealth.OnHealthChangedEvent += HandleHealthChange;
            InputReader.OnDashEvent += HandleDashEvent;
            InputReader.OnInteractEvent += HandleInteractEvent;
            InputReader.OnSprintToggleEvent += HandleSprintToggle;

            //InputReader.OnReloadEvent += _weaponGunHolderCompo.Reload; //재장전 만들꺼면 다시 구현
            InputReader.OnAttackEvent += HandleAttackEvent;
        }

        private void Start()
        {
            TargetPointManager.Instance.ShowTargetPoint(transform, Color.cyan);

            _currentStamina = MaxStamina;
            OnStaminaChangedEvent?.Invoke(_currentStamina, _currentStamina);
        }

        private void OnDisable()
        {
            if (TargetPointManager.Instance != null)
                TargetPointManager.Instance.CloseTargetPoint(transform);
        }

        #region Handles

        private void HandleHealthChange(int old, int cur)
        {
            if (old > cur)
            {
                CameraManager.Instance.ShakeCamera(5, 5, 0.3f);
                CameraManager.Instance.Aberration(1f, 0.1f);
            }
        }

        private void HandleInteractEvent(bool isDown)
        {
            _interactor.Interact(isDown);
        }

        private void HandleSprintToggle()
        {
            _playerMover.OnSprintToggle();
            _stateMachine.ChangeState(_playerMover.IsSprint ? "Run" : "Walk");
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

        private void HandleAttackEvent(bool isDown)
        {
            //공격
        }

        #endregion

        protected override void InitializeComponent()
        {
            Attacker = FindAnyObjectByType<Attacker>();
            base.InitializeComponent();

            _playerMover = GetEntityComponent<PlayerMover>();
            _statCompo = GetEntityComponent<EntityStat>();
            _renderCompo = GetEntityComponent<EntityRenderer>();
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


            if (_isRightMousePress && _isChargingParrying == false &&
                _lastRightClickTime + _chargingParryingStartDelay < Time.time)
            {
                _isChargingParrying = true;
                if (GameManager.Instance.Volume.profile.TryGet(out ChromaticAberration chromaticAberration))
                {
                    chromaticAberration.active = true;
                    chromaticAberration.intensity.value = 1f;
                }
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

            _entityHealth.OnHealthChangedEvent -= HandleHealthChange;
            InputReader.OnDashEvent -= HandleDashEvent;
            InputReader.OnInteractEvent -= HandleInteractEvent;
            InputReader.OnSprintToggleEvent -= HandleSprintToggle;

            //InputReader.OnReloadEvent -= _weaponGunHolderCompo.Reload; //재장전시 구현
            InputReader.OnAttackEvent -= HandleAttackEvent;
        }
    }
}