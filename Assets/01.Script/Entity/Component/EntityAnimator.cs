using Hashira.Items.Weapons;
using Hashira.Core.AnimationSystem;
using Hashira.Players;
using System;
using UnityEngine;
using UnityEngine.U2D.IK;

namespace Hashira.Entities.Components
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IDisposable
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [SerializeField] private AnimatorParamSO _moveDirParamSO;
        [SerializeField] private LimbSolver2D _rightHandSolver;
        [SerializeField] private LimbSolver2D _leftHandSolver;
        [SerializeField] private IKManager2D _ikManager;

        private Entity _entity;
        private EntityMover _mover;
        private EntityRenderer _renderer;
        private EntityWeapon _weapon;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void AfterInit()
        {
            _mover = _entity.GetEntityComponent<PlayerMover>();
            _renderer = _entity.GetEntityComponent<EntityRenderer>();
            _weapon = _entity.GetEntityComponent<EntityWeapon>();
            if (_weapon != null) _weapon.OnCurrentWeaponChanged += HandleCurrentWeaponChangedEvnet;
        }

        private void HandleCurrentWeaponChangedEvnet(Weapon weapon)
        {
            if (_rightHandSolver != null && _leftHandSolver != null)
            {
                if (weapon == null)
                {
                    _rightHandSolver.weight = 0;
                    _leftHandSolver.weight = 0;
                }
                else
                {
                    _rightHandSolver.weight = 1;
                    _leftHandSolver.weight = 1;
                    _rightHandSolver.GetChain(0).target.localPosition = weapon.WeaponSO.RightHandOffset;
                    _leftHandSolver.GetChain(0).target.localPosition = weapon.WeaponSO.LeftHandOffset;
                }
            }
        }

        private void Update()
        {
            if (_mover != null && _renderer != null)
            {
                float moveDir = Mathf.Sign(_mover.Velocity.x) * _renderer.FacingDirection;
                SetParam(_moveDirParamSO, moveDir);
            }
        }
        public void Dispose()
        {
            if (_weapon != null) _weapon.OnCurrentWeaponChanged -= HandleCurrentWeaponChangedEvnet;
        }

        #region Param Funcs
        public void SetParam(AnimatorParamSO param, float value) => Animator.SetFloat(param.hash, value);
        public void SetParam(AnimatorParamSO param, int value) => Animator.SetInteger(param.hash, value);
        public void SetParam(AnimatorParamSO param, bool value) => Animator.SetBool(param.hash, value);
        public void SetParam(AnimatorParamSO param) => Animator.SetTrigger(param.hash);
        #endregion
    }
}
