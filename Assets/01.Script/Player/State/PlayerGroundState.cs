using Doryu.StatSystem;
using System;
using UnityEngine;

public class PlayerGroundState : EntityState<Player>
{
    protected MoverCompo _moverCompo;
    protected StatElement _jumpStat;

    public PlayerGroundState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
    {
        _moverCompo = owner.GetEntityComponent<MoverCompo>();
        _jumpStat = owner.GetEntityComponent<StatCompo>().GetElement("JumpPower");
    }

    public override void Enter()
    {
        base.Enter();

        _owner.InputReader.OnJumpEvent += HandleJumpEvent;
        _owner.InputReader.OnCrouchEvent += HandleCrouchEvent;
    }

    private void HandleCrouchEvent(bool isOn)
    {
        if (isOn)
            _stateMachine.ChangeState(EPlayerState.Crouch);
        else
            _stateMachine.ChangeState(EPlayerState.Idle);
    }

    private void HandleJumpEvent()
    {
        _moverCompo.Jump(_jumpStat == null ? 0 : _jumpStat.Value);
    }

    public override void Update()
    {
        base.Update();

        if (_moverCompo.IsGround == false)
            _stateMachine.ChangeState(EPlayerState.Air);
    }

    public override void Exit()
    {
        base.Exit();

        _owner.InputReader.OnJumpEvent -= HandleJumpEvent;
        _owner.InputReader.OnCrouchEvent -= HandleCrouchEvent;
    }
}
