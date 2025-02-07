using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerMover : EntityMover
    {
        public bool CanRolling => _currentDelayTime >= rollingDelay;
        public float rollingDelay = 1;
        private float _currentDelayTime = 0;

		private void Update()
		{
            if(_currentDelayTime < rollingDelay)
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