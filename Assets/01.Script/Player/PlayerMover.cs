using Hashira.Entities;
using Hashira.FSM;
using Hashira.MainScreen;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerMover : EntityMover
    {
        public bool CanRolling => _currentDelayTime >= rollingDelay;
        public float rollingDelay = 1;
        private float _currentDelayTime = 0;
		public bool IsSprint { get; private set; } = false;

        private float _lastVelocityValue;

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

		public void OnSprintToggle()
		{
			IsSprint = !IsSprint;
		}

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _lastVelocityValue = Rigidbody2D.linearVelocity.magnitude;
        }

        public void OnCollision(Collision2D collision)
        {
            if (_lastVelocityValue > 1)
            {
                MainScreenEffect.OnWallShockWaveEffect(transform.position);
            }
            Debug.Log(_lastVelocityValue);
        }
    }
}