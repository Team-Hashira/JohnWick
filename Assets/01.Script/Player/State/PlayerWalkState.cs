using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;

namespace Hashira.Players
{
    public class PlayerWalkState : PlayerGroundState
    {
        private StatElement _speedStat;

        public PlayerWalkState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
        {
            _speedStat = owner.GetEntityComponent<EntityStat>().StatDictionary["Speed"];
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            float movement = _owner.InputReader.XMovement;
            if (movement != 0)
            {
                if (_speedStat != null)
                    movement *= _speedStat.Value;
                _entityMover.SetMovement(movement);
            }
            else
                _stateMachine.ChangeState(EPlayerState.Idle);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}