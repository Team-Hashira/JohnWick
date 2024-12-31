using Doryu.StatSystem;
using UnityEngine;

public class PlayerAirState : EntityState<Player>
{
    private StatElement _speedStat;
    private StatElement _sprintSpeedStat;
    protected MoverCompo _moverCompo;

    public PlayerAirState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
    {
        _moverCompo = owner.GetEntityComponent<MoverCompo>();
        _speedStat = owner.GetEntityComponent<StatCompo>().GetElement("Speed");
        _sprintSpeedStat = owner.GetEntityComponent<StatCompo>().GetElement("SprintSpeed");
    }

    public override void Update()
    {
        base.Update();

        float movement = _owner.InputReader.XMovement;
        if (_owner.InputReader.IsSprint && _sprintSpeedStat != null)
            movement *= _sprintSpeedStat.Value;
        else if (_speedStat != null)
                movement *= _speedStat.Value;

        _moverCompo.SetMovement(movement);

        if (_moverCompo.IsGround == true)
            _stateMachine.ChangeState(EPlayerState.Idle);
    }
}
