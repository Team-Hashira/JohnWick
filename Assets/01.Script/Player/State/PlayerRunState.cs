using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;

namespace Hashira.Players
{
	public class PlayerRunState : PlayerGroundState
	{
		private PlayerMover _playerMover;
		private EntitySoundGenerator _soundGenerator;
		private StatElement _sprintSpeedStat;

		public PlayerRunState(Entity entity, StateSO stateSO) : base(entity, stateSO)
		{
			_playerMover = entity.GetEntityComponent<PlayerMover>();
			_soundGenerator = entity.GetEntityComponent<EntitySoundGenerator>();
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

				float loudness = 1f;
				_soundGenerator.SoundGenerate(loudness);
			}
			else
				_entityStateMachine.ChangeState("Idle");

			if(!_playerMover.IsSprint)
				_entityStateMachine.ChangeState("Walk");
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}
