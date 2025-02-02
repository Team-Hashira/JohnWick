using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.LatestFSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerRollingState : EntityState
    {
        private StatElement _dashSpeedStat;
        private EntityMover _entityMover;
        private EntityRenderer _entityRenderer;
        private Player _player;

        private float _moveDir;

        public PlayerRollingState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _player = entity as Player;
            _entityMover = entity.GetEntityComponent<EntityMover>(true);
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _dashSpeedStat = entity.GetEntityComponent<EntityStat>().StatDictionary["DashSpeed"];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (_entityMover.Velocity.x != 0)
                _moveDir = Mathf.Sign(_entityMover.Velocity.x);
            else
                _moveDir = _entityRenderer.FacingDirection;

            _player.AfterImageParticle.Play();

            _entityAnimator.OnAnimationTriggeredEvent += HandleAnimationTriggerEvent;
            _entityRenderer.SetArmActive(false);
            _entityRenderer.isUsualFacing = false;
        }

        private void HandleAnimationTriggerEvent(EAnimationTriggerType type, int count)
        {
            if (type == EAnimationTriggerType.End)
            {
                if (_player.InputReader.XMovement != 0)
                    _entityStateMachine.ChangeState("Walk");
                else
                    _entityStateMachine.ChangeState("Idle");
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            _entityMover.SetMovement(_moveDir * 17f);
            _entityRenderer.LookTarget(_player.transform.position + Vector3.right * _moveDir * _entityRenderer.FacingDirection);
        }

        public override void OnExit()
        {
            base.OnExit();
            _entityAnimator.OnAnimationTriggeredEvent -= HandleAnimationTriggerEvent;
            _entityRenderer.SetArmActive(true);
            _entityRenderer.isUsualFacing = true;
        }
    }
}