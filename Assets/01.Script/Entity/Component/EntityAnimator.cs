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
        private readonly static int _MoveDirAnimationHash = Animator.StringToHash("MoveDir");

        [field: SerializeField] public Animator Animator { get; private set; }
        [SerializeField] private Transform _rightHand;
        [SerializeField] private Transform _leftHand;

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
            if (_rightHand != null && _leftHand != null)
            {
                if (weapon == null)
                {
                    _rightHand.localPosition = Vector3.down * 5f;
                    _leftHand.localPosition = Vector3.down * 5f;
                }
                else
                {
                    _rightHand.localPosition = weapon.WeaponSO.RightHandOffset;
                    _leftHand.localPosition = weapon.WeaponSO.LeftHandOffset;
                }
            }
        }

        private void Update()
        {
            if (_mover != null && _renderer != null)
            {
                float moveDir = Mathf.Sign(_mover.Velocity.x) * _renderer.FacingDirection;
                SetParam(_MoveDirAnimationHash, moveDir);
            }
        }
        public void Dispose()
        {
            if (_weapon != null) _weapon.OnCurrentWeaponChanged -= HandleCurrentWeaponChangedEvnet;
        }

        #region Param Func
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
