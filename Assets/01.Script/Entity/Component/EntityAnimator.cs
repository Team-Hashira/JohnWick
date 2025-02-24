using Hashira.Core.AnimationSystem;
using Hashira.Items;
using Hashira.Players;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

namespace Hashira.Entities.Components
{
    public enum EAnimationTriggerType
    {
        Start,
        Trigger,
        End,
    }

    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IDisposable
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [SerializeField] private AnimatorParamSO _moveDirParamSO;
        [SerializeField] private AnimatorParamSO _yVelocityParamSO;
        [SerializeField] private LimbSolver2D _rightHandSolver;
        [SerializeField] private LimbSolver2D _leftHandSolver;

        [Header("-----IK Targets")]
        [SerializeField] private Transform _targetEmptyR;
        [SerializeField] private Transform _targetEmptyL;
        [SerializeField] private Transform _targetWeaponR;
        [SerializeField] private Transform _targetWeaponL;

        private Entity _entity;
        private EntityMover _mover;
        private EntityRenderer _renderer;
        private EntityWeaponHolder _weapon;

        private Dictionary<EAnimationTriggerType, int> _triggerDictionary = new();
        public event Action<EAnimationTriggerType, int> OnAnimationTriggeredEvent;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void AfterInit()
        {
            _mover = _entity.GetEntityComponent<PlayerMover>();
            _renderer = _entity.GetEntityComponent<EntityRenderer>();
            _weapon = _entity.GetEntityComponent<EntityWeaponHolder>();
            if (_weapon != null) _weapon.OnCurrentWeaponChanged += HandleCurrentItemChangedEvnet;
        }

        private void HandleCurrentItemChangedEvnet(Item item)
        {
            if (item is Weapon weapon && _rightHandSolver != null && _leftHandSolver != null)
            {
                if (weapon == null)
                {
                    _rightHandSolver.GetChain(0).target = _targetEmptyR;
                    _leftHandSolver.GetChain(0).target = _targetEmptyL;
                }
                else
                {
                    _rightHandSolver.GetChain(0).target = _targetWeaponR;
                    _leftHandSolver.GetChain(0).target = _targetWeaponL;

                    _rightHandSolver.GetChain(0).target.localPosition = weapon.WeaponSO.RightHandOffset;
                    _leftHandSolver.GetChain(0).target.localPosition = weapon.WeaponSO.LeftHandOffset;
                }
            }
        }

        private void AnimationTrigger(EAnimationTriggerType eAnimationTriggerType)
        {
            if (_triggerDictionary.ContainsKey(eAnimationTriggerType))
                _triggerDictionary[eAnimationTriggerType]++;
            else
                _triggerDictionary[eAnimationTriggerType] = 1;
            OnAnimationTriggeredEvent?.Invoke(eAnimationTriggerType, _triggerDictionary[eAnimationTriggerType]);
        }

        public void ClearAnimationTriggerDictionary()
            => _triggerDictionary.Clear();

        private void Update()
        {
            if (_mover != null && _renderer != null)
            {
                float xVelocity = Mathf.Sign(_mover.Velocity.x) * _renderer.FacingDirection;
                SetParam(_moveDirParamSO, xVelocity);
                SetParam(_yVelocityParamSO, _mover.Velocity.y);
            }
        }
        public void Dispose()
        {
            if (_weapon != null) _weapon.OnCurrentWeaponChanged -= HandleCurrentItemChangedEvnet;
        }

        #region Param Funcs
        public void SetParam(int hash, float value) => Animator.SetFloat(hash, value);
        public void SetParam(int hash, int value) => Animator.SetInteger(hash, value);
        public void SetParam(int hash, bool value) => Animator.SetBool(hash, value);
        public void SetParam(int hash) => Animator.SetTrigger(hash);

        public void SetParam(AnimatorParamSO param, float value) => Animator.SetFloat(param.hash, value);
        public void SetParam(AnimatorParamSO param, int value) => Animator.SetInteger(param.hash, value);
        public void SetParam(AnimatorParamSO param, bool value) => Animator.SetBool(param.hash, value);
        public void SetParam(AnimatorParamSO param) => Animator.SetTrigger(param.hash);
        #endregion
    }
}
