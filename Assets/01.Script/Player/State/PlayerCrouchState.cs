using System.Collections;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerCrouchState : PlayerGroundState
    {
        private readonly InputReaderSO _inputReader;
        private PlayerMover _playerMover;
        public PlayerCrouchState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
        {
            _inputReader = owner.InputReader;
            _playerMover = owner.GetEntityComponent<PlayerMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _entityMover.StopImmediately();
            _owner.VisualTrm.localScale = new Vector3(1f, 0.8f, 1f);
        }

        public override void Exit()
        {
            base.Exit();
            _playerMover.UnderJump(false);
            _owner.VisualTrm.localScale = new Vector3(1f, 1f, 1f);
        }

        protected override void HandleJumpEvent()
        {
            _playerMover.UnderJump(true);
        }
    }
}
