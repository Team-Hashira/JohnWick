using Hashira.Entities;
using Hashira.LatestFSM;

namespace Hashira.Players
{
	public class PlayerHitState : EntityState
	{
		private Player _player;
		private EntityHealth _entityHealth;

		public PlayerHitState(Entity entity, StateSO stateSO) : base(entity, stateSO)
		{
			_player = entity as Player;
			_entityHealth = _player.GetEntityComponent<EntityHealth>();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if(_entityHealth.IsKnockback == false)
			{
				_entityStateMachine.ChangeState("Idle");
			}
		}
	}
}
