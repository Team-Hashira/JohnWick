using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerAirState : EntityState<Player>
    {
        private StatElement _speedStat;
        protected EntityMover _entityMover;

        public PlayerAirState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
        {
            _entityMover = owner.GetEntityComponent<EntityMover>(true);
            _speedStat = owner.GetEntityComponent<EntityStat>().StatDictionary["Speed"];
        }

        public override void Update()
        {
            base.Update();

            float movement = _owner.InputReader.XMovement;
            if (_speedStat != null)
                movement *= _speedStat.Value;

            _entityMover.SetMovement(movement);

            if (_entityMover.IsGrounded == true)
                _stateMachine.ChangeState(EPlayerState.Idle);
        }
    }
}