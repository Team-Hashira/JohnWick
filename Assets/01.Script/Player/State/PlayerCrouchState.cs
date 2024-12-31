using UnityEngine;

public class PlayerCrouchState : PlayerGroundState
{
    public PlayerCrouchState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _moverCompo.StopImmediant();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
