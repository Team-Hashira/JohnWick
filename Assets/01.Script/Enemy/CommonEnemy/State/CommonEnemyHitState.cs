using Hashira.Entities;
using Hashira.FSM;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemyHitState : EntityState
    {
		private EntityHealth _entityHealth;
		private EnemyPathfinder _enemyPathfinder;

		public CommonEnemyHitState(Entity entity, StateSO stateSO) : base(entity, stateSO)
		{
			_entityHealth = entity.GetEntityComponent<EntityHealth>();
			_enemyPathfinder = entity.GetEntityComponent<EnemyPathfinder>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _entityStateMachine.SetShareVariable("Target", GameManager.Instance.Player);
			_enemyPathfinder.StopMove();
        }

        public override void OnUpdate()
		{
			base.OnUpdate();
			if (_entityHealth.IsKnockback == false)
				_entityStateMachine.ChangeState("Chase");
		}
	}
}
