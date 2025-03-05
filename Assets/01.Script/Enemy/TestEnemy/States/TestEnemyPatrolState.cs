using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies.TestEnemy
{
    public class TestEnemyPatrolState : EntityState
    {
        private float _moveTime = 2f;
        private float _moveTimer = 0;

        private EntityRenderer _entityRenderer;
        private EntityStat _entityStat;
        private EnemyMover _enemyMover;

        private StatElement _speedElement;

        public TestEnemyPatrolState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _entityStat = entity.GetEntityComponent<EntityStat>();
            _enemyMover = entity.GetEntityComponent<EnemyMover>();

            _speedElement = _entityStat.StatDictionary["Speed"];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _moveTimer = 0;
            _entityRenderer.Flip();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _enemyMover.SetMovement(_entityRenderer.FacingDirection * _speedElement.Value);
            _moveTimer += Time.deltaTime;
            if (_moveTimer >= _moveTime)
            {
                _entityStateMachine.ChangeState("Idle");
            }
        }
    }
}