using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerDashState : EntityState<Player>
    {
        private StatElement _dashSpeedStat;
        private EntityMover _entityMover;

        float _dashStartTime;
        float _dashDuration = 0.1f;

        public PlayerDashState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
        {
            _entityMover = owner.GetEntityComponent<EntityMover>(true);
            _dashSpeedStat = owner.GetEntityComponent<EntityStat>().StatDictionary["DashSpeed"];
        }

        public override void Enter()
        {
            base.Enter();
            _dashStartTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            float movement = _owner.InputReader.XMovement;
            if (_dashSpeedStat != null)
                movement *= _dashSpeedStat.Value;
            _entityMover.SetMovement(movement);

            if (_dashStartTime + _dashDuration < Time.time)
            {
                if (_owner.InputReader.XMovement != 0)
                    _stateMachine.ChangeState(EPlayerState.Walk);
                else
                    _stateMachine.ChangeState(EPlayerState.Idle);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}