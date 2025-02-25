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
        private EnemyMover _enemyMover;

        public TestEnemyPatrolState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
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
            _enemyMover.SetMovement(_entityRenderer.FacingDirection * 5);
            _moveTimer += Time.deltaTime;
            if (_moveTimer >= _moveTime)
            {
                _entityStateMachine.ChangeState("Idle");
            }
        }
    }
}