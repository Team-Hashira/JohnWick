using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies
{
    public class EnemyPatrolState : ListeningSoundState
    {
        private EnemyMover _enemyMover;
        private EntityRenderer _entityRenderer;
        private EntityStat _entityStat;

        private StatElement _speedElement;

        public EnemyPatrolState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _entityStat = entity.GetEntityComponent<EntityStat>();
            _speedElement = _entityStat?.StatDictionary["Speed"];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _entityRenderer.Flip();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(_enemyMover.IsOnEdge() || _enemyMover.IsWallOnFront())
            {
                _entityStateMachine.ChangeState("Idle");
                return;
            }
            _enemyMover.SetMovement(_entityRenderer.FacingDirection * _speedElement.Value);
        }
    }
}
