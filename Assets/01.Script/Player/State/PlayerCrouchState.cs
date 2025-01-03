using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerCrouchState : PlayerGroundState
    {
        public PlayerCrouchState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
        {
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
            _owner.VisualTrm.localScale = new Vector3(1f, 1f, 1f);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
