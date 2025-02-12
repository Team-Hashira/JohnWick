using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;

namespace Hashira.Players
{
	public class PlayerRunState : PlayerGroundState
	{
		private StatElement _sprintSpeedStat;

		public PlayerRunState(Entity entity, StateSO stateSO) : base(entity, stateSO)
		{
			_sprintSpeedStat = entity.GetEntityComponent<EntityStat>().StatDictionary["SprintSpeed"];
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
				if (_sprintSpeedStat != null)
					movement *= _sprintSpeedStat.Value;
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
