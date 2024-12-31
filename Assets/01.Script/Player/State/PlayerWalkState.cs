using Doryu.StatSystem;
using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    private StatElement _speedStat;

    public PlayerWalkState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
    {
        _speedStat = owner.GetEntityComponent<StatCompo>().GetElement("Speed");
    }

    public override void Update()
    {
        base.Update();

        if (_owner.InputReader.XMovement != 0)
        {
            float movement = _owner.InputReader.XMovement;
            if (_speedStat != null)
                movement *= _speedStat.Value;
            _moverCompo.SetMovement(movement);
        }
        else
        {
            _stateMachine.ChangeState(EPlayerState.Idle);
        }
    }
}
