using Doryu.StatSystem;
using System;
using UnityEngine;

public class PlayerSprintState : PlayerGroundState
{
    private StatElement _sprintSpeedStat;

    public PlayerSprintState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
    {
        _sprintSpeedStat = owner.GetEntityComponent<StatCompo>().GetElement("SprintSpeed");
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (_owner.InputReader.XMovement != 0)
        {
            if (_owner.InputReader.IsSprint)
            {
                float movement = _owner.InputReader.XMovement;
                if (_sprintSpeedStat != null)
                    movement *= _sprintSpeedStat.Value;
                _moverCompo.SetMovement(movement);
            }
            else
            {
                _stateMachine.ChangeState(EPlayerState.Walk);
            }
        }
        else
        {
            _stateMachine.ChangeState(EPlayerState.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
