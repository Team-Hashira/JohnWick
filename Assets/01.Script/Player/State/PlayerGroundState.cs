using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.LatestFSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerGroundState : EntityState
    {
        private readonly static int _JumpingAnimationHash = Animator.StringToHash("Jumping");
        protected EntityMover _entityMover;
        protected StatElement _jumpStat;
        protected Player _player;

        public PlayerGroundState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _player = entity as Player;
            _entityMover = entity.GetEntityComponent<EntityMover>(true);
            _jumpStat = entity.GetEntityComponent<EntityStat>().StatDictionary["JumpPower"];
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _player.InputReader.OnJumpEvent += HandleJumpEvent;
            _player.InputReader.OnCrouchEvent += HandleCrouchEvent;
        }

        private void HandleDashEvent()
        {
            _entityStateMachine.ChangeState("Dash");
        }

        private void HandleCrouchEvent(bool isOn)
        {
            if (isOn)
                _entityStateMachine.ChangeState("Crouch");
            else
                _entityStateMachine.ChangeState("Idle");
        }

        protected virtual void HandleJumpEvent()
        {
            _entityMover.Jump(_jumpStat == null ? 0 : _jumpStat.Value);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_entityMover.IsGrounded == false)
            {
                _entityStateMachine.ChangeState("Air");
                _entityAnimator.SetParam(_JumpingAnimationHash);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            _player.InputReader.OnJumpEvent -= HandleJumpEvent;
            _player.InputReader.OnCrouchEvent -= HandleCrouchEvent;
            _player.InputReader.OnDashEvent -= HandleDashEvent;
        }
    }
}