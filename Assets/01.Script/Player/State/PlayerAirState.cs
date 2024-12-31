using Doryu.StatSystem;
using UnityEngine;

public class PlayerAirState : EntityState<Player>
{
    private StatElement _speedStat;
    protected MoverCompo _moverCompo;

    public PlayerAirState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
    {
        _moverCompo = owner.GetEntityComponent<MoverCompo>();
        _speedStat = owner.GetEntityComponent<StatCompo>().GetElement("Speed");
    }

    public override void Update()
    {
        base.Update();

        float movement = _owner.InputReader.XMovement;
        if (_speedStat != null)
            movement *= _speedStat.Value * 0.8f;
        _moverCompo.SetMovement(movement);

        if (_moverCompo.IsGround == true)
            _stateMachine.ChangeState(EPlayerState.Idle);
    }
}
