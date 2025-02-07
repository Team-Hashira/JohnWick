using Hashira.Entities;
using Hashira.LatestFSM;

namespace Hashira.Enemies
{
    public class EnemyHitState : EntityState
    {
		private EntityHealth _entityHealth;

		public EnemyHitState(Entity entity, StateSO stateSO) : base(entity, stateSO)
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
