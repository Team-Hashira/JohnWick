using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using System;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerGroundState : EntityState<Player>
    {
        protected EntityMover _entityMover;
        protected StatElement _jumpStat;

        public PlayerGroundState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
        {
            _entityMover = owner.GetEntityComponent<EntityMover>(true);
            _jumpStat = owner.GetEntityComponent<EntityStat>().StatDictionary["JumpPower"];
        }

        public override void Enter()
        {
            base.Enter();

            _owner.InputReader.OnJumpEvent += HandleJumpEvent;
            _owner.InputReader.OnCrouchEvent += HandleCrouchEvent;
        }

        private void HandleDashEvent()
        {
            _stateMachine.ChangeState(EPlayerState.Dash);
        }

        private void HandleCrouchEvent(bool isOn)
        {
            if (isOn)
                _stateMachine.ChangeState(EPlayerState.Crouch);
            else
                _stateMachine.ChangeState(EPlayerState.Idle);
        }

        protected virtual void HandleJumpEvent()
        {
            _entityMover.Jump(_jumpStat == null ? 0 : _jumpStat.Value);
        }

        public override void Update()
        {
            base.Update();

            if (_entityMover.IsGrounded == false)
                _stateMachine.ChangeState(EPlayerState.Air);
        }

        public override void Exit()
        {
            base.Exit();

            _owner.InputReader.OnJumpEvent -= HandleJumpEvent;
            _owner.InputReader.OnCrouchEvent -= HandleCrouchEvent;
            _owner.InputReader.OnDashEvent -= HandleDashEvent;
        }
    }
}