using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Stage.Area
{
    public class TriggerArea : MonoBehaviour
    {
		public UnityEvent Event;

        public LayerMask whatIsTarget;
        public Vector2 size = Vector2.one;

		public bool isOnlyOnce = true;
		private bool isTrigged = false;

		public void FixedUpdate()
		{
			if (Physics2D.OverlapBox(transform.position, size, transform.eulerAngles.z, whatIsTarget))
			{
				isTrigged = true;
				if (isOnlyOnce && isTrigged) 
					Event?.Invoke();
			}
		}

		private void OnDrawGizmos()
		{
			var origin = Gizmos.matrix;
			Gizmos.DrawWireCube(transform.position, size);
		}
	}
}
