using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerAirState : EntityState<Player>
    {
        private StatElement _speedStat;
        private StatElement _sprintSpeedStat;
        protected EntityMover _entityMover;

        public PlayerAirState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
        {
            _entityMover = owner.GetEntityComponent<EntityMover>();
            _speedStat = owner.GetEntityComponent<EntityStat>().GetElement("Speed");
            _sprintSpeedStat = owner.GetEntityComponent<EntityStat>().GetElement("SprintSpeed");
        }

        public override void Update()
        {
            base.Update();

            float movement = _owner.InputReader.XMovement;
            if (_owner.InputReader.IsSprint && _sprintSpeedStat != null)
                movement *= _sprintSpeedStat.Value;
            else if (_speedStat != null)
                movement *= _speedStat.Value;

            _entityMover.SetMovement(movement);

            if (_entityMover.IsGrounded == true)
                _stateMachine.ChangeState(EPlayerState.Idle);
        }
    }
}