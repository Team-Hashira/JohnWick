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

		private float _curTime = 0;
		private readonly float _soundDelay = 0.2f;

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
			_curTime += Time.deltaTime;
			float movement = _player.InputReader.XMovement;
			float look = _entityRenderer.FacingDirection;

			if (movement != 0)
			{
				if (_sprintSpeedStat != null)
					movement *= _sprintSpeedStat.Value;
				_playerMover.SetMovement(movement);
				if(_curTime > _soundDelay)
					OnSoundGenerate();
			}
			else
				_entityStateMachine.ChangeState("Idle");

			if(!_playerMover.IsSprint || Mathf.Sign(movement) != Mathf.Sign(look))
				_entityStateMachine.ChangeState("Walk");
		}

		private void OnSoundGenerate()
		{
			_curTime = 0;
			float loudness = 2f;
			_soundGenerator.SoundGenerate(loudness, new Vector3(0, -0.75f));
		}

		public override void OnExit()
		{
			_curTime = 0;
			base.OnExit();
		}
	}
}
