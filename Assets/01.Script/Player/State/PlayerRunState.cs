using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
	public class PlayerRunState : PlayerGroundState
	{
		private EntitySoundGenerator _soundGenerator;
		private StatElement _sprintSpeedStat;

		public PlayerRunState(Entity entity, StateSO stateSO) : base(entity, stateSO)
		{
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
			float look = _entityRenderer.FacingDirection;

			if (movement != 0)
			{
				if (_sprintSpeedStat != null)
					movement *= _sprintSpeedStat.Value;
				_playerMover.SetMovement(movement);

				float loudness = 1f;
				_soundGenerator.SoundGenerate(loudness);
			}
			else
				_entityStateMachine.ChangeState("Idle");

			if(!_playerMover.IsSprint || Mathf.Sign(movement) != Mathf.Sign(look))
				_entityStateMachine.ChangeState("Walk");
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}
