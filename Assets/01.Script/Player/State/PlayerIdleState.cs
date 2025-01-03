using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerIdleState : PlayerGroundState
    {
        public PlayerIdleState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
        {

        }

        public override void Enter()
        {
            base.Enter();

            _entityMover.StopImmediately();


        }

        public override void Update()
        {
            base.Update();

            if (_owner.InputReader.XMovement != 0)
            {
                _stateMachine.ChangeState(EPlayerState.Walk);
            }
        }
    }
}