using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;

namespace Hashira.Players
{
    public class PlayerWalkState : PlayerGroundState
    {
        private StatElement _speedStat;

        public PlayerWalkState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _speedStat = entity.GetEntityComponent<EntityStat>().StatDictionary["Speed"];
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            float movement = _player.InputReader.XMovement;
            if (movement != 0)
            {
                if (_speedStat != null)
                    movement *= _speedStat.Value;
                _entityMover.SetMovement(movement);
            }
            else
                _entityStateMachine.ChangeState("Idle");
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}