using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.LatestFSM;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.EventSystems.EventTrigger;

namespace Hashira.Players
{
    public class PlayerAirState : EntityState
    {
        private StatElement _speedStat;
        protected EntityMover _entityMover;

        private Player _player;

        public PlayerAirState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _player = entity as Player;
            _entityMover = entity.GetEntityComponent<EntityMover>(true);
            _speedStat = entity.GetEntityComponent<EntityStat>().StatDictionary["Speed"];
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
    }
}