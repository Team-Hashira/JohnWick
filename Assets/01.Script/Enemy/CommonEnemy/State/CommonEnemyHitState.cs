using Hashira.Entities;
using Hashira.FSM;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemyHitState : EntityState
    {
		private EntityHealth _entityHealth;

		public CommonEnemyHitState(Entity entity, StateSO stateSO) : base(entity, stateSO)
		{
			_entityHealth = entity.GetEntityComponent<EntityHealth>();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (_entityHealth.IsKnockback == false)
				_entityStateMachine.ChangeState("Idle");
		}
	}
}
