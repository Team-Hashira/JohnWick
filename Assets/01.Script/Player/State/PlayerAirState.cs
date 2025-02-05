using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.LatestFSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerAirState : EntityState
    {
        private readonly static int _LandingAnimationHash = Animator.StringToHash("Landing");
        private StatElement _speedStat;
        protected EntityMover _entityMover;
		protected StatElement _jumpStat;

		private Player _player;

        private bool _isJumped = false;

        public PlayerAirState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _player = entity as Player;
            _entityMover = entity.GetEntityComponent<EntityMover>(true);
            _speedStat = entity.GetEntityComponent<EntityStat>().StatDictionary["Speed"];
			_jumpStat = entity.GetEntityComponent<EntityStat>().StatDictionary["JumpPower"];
		}

		public override void OnEnter()
		{
			base.OnEnter();
			_player.InputReader.OnJumpEvent += HandleJumpEvent;
		}

		public override void OnUpdate()
        {
            base.OnUpdate();

            float movement = _player.InputReader.XMovement;
            if (_speedStat != null)
                movement *= _speedStat.Value;

            _entityMover.SetMovement(movement);

            if (_entityMover.IsGrounded == true)
                _entityStateMachine.ChangeState("Idle");
        }

		public override void OnExit()
        {
            base.OnExit();
            _entityAnimator.SetParam(_LandingAnimationHash);
			_player.InputReader.OnJumpEvent -= HandleJumpEvent;
            _isJumped = false;
		}

		protected virtual void HandleJumpEvent()
		{
            if (_isJumped == true) return;
            _isJumped = true;
			_entityMover.Jump(_jumpStat == null ? 0 : _jumpStat.Value);
		}
	}
}