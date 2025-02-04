using Hashira.Entities;
using Hashira.LatestFSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerMover : EntityMover
    {
        public bool CanDash => _currentDelayTime >= dashDelay;
        public float dashDelay = 1;
        private float _currentDelayTime = 0;

		private void Update()
		{
            if(_currentDelayTime < dashDelay)
                _currentDelayTime += Time.deltaTime;
		}

		public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
        }

        public void OnDash()
        {
            _currentDelayTime = 0;
		}
    }
}