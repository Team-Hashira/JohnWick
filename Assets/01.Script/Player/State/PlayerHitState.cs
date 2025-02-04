using Hashira.Entities;
using Hashira.LatestFSM;

namespace Hashira.Players
{
	public class PlayerHitState : EntityState
	{
		private Player _player;

		public PlayerHitState(Entity entity, StateSO stateSO) : base(entity, stateSO)
		{
			_player = entity as Player;
		}
	}
}
