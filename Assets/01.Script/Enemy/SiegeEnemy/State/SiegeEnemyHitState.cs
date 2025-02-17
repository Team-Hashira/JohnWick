using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies.SiegeEnemy
{
    public class SiegeEnemyHitState : EntityState
    {
        private EntityHealth _entityHealth;
        private EnemyMover _enemyMover;

        public SiegeEnemyHitState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _entityHealth = entity.GetEntityComponent<EntityHealth>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyMover.StopImmediately();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!_entityHealth.IsKnockback)
            {
                _entityStateMachine.ChangeState("Attack");
            }
        }
        public override void OnExit()
        {
            _enemyMover.StopImmediately();
            base.OnExit();
        }
    }
}
