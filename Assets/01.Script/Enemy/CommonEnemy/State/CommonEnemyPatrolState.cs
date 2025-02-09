using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemyPatrolState : EnemyListeningSoundState
    {
        private CommonEnemy _enemy;

        private EnemyMover _enemyMover;
        private EntityRenderer _entityRenderer;
        private EntityStat _entityStat;

        private StatElement _speedElement;

        public CommonEnemyPatrolState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _entityStat = entity.GetEntityComponent<EntityStat>();
            _speedElement = _entityStat?.StatDictionary["Speed"];
            _enemy = entity as CommonEnemy;
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
            Player player = _enemy.DetectPlayer();
            if (player != null)
            {
                _entityStateMachine.SetShareVariable("Target", player);
                _entityStateMachine.ChangeState("Chase");
            }
        }
    }
}
