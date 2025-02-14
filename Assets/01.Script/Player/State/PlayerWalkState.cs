using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;

namespace Hashira.Players
{
    public class PlayerWalkState : PlayerGroundState
    {
		private PlayerMover _playerMover;
		private StatElement _speedStat;

        public PlayerWalkState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
			_playerMover = entity.GetEntityComponent<PlayerMover>();
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

            if(_playerMover.IsSprint)
				_entityStateMachine.ChangeState("Run");

		}

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}