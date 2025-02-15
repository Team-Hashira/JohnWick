using Hashira.Entities;
using Hashira.FSM;

namespace Hashira.Players
{
    public class PlayerIdleState : PlayerGroundState
    {
        private PlayerMover _playerMover;

		public PlayerIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
			_playerMover = entity.GetEntityComponent<PlayerMover>();
		}

        public override void OnEnter()
        {
            base.OnEnter();
            _playerMover.StopImmediately();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_player.InputReader.XMovement != 0)
            {
                if(_playerMover.IsSprint)
					_entityStateMachine.ChangeState("Run");
				else
					_entityStateMachine.ChangeState("Walk");
            }
        }
    }
}