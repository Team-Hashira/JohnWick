using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.LatestFSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerDashState : EntityState
    {
        private StatElement _dashSpeedStat;
        private EntityMover _entityMover;
        private Player _player;

        private float _dashStartTime;
        private float _dashDuration = 0.1f;

        public PlayerDashState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _player = entity as Player;
            _entityMover = entity.GetEntityComponent<EntityMover>(true);
            _dashSpeedStat = entity.GetEntityComponent<EntityStat>().StatDictionary["DashSpeed"];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _dashStartTime = Time.time;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            float movement = _player.InputReader.XMovement;
            if (_dashSpeedStat != null)
                movement *= _dashSpeedStat.Value;
            _entityMover.SetMovement(movement);

            if (_dashStartTime + _dashDuration < Time.time)
            {
                if (_player.InputReader.XMovement != 0)
                    _entityStateMachine.ChangeState("Walk");
                else
                    _entityStateMachine.ChangeState("Idle");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}