using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Stage.Area
{
    public class TriggerArea : MonoBehaviour
    {
		public UnityEvent Event;

        public LayerMask whatIsTarget;
        public Vector2 size = Vector2.one;

		public void FixedUpdate()
		{
			if (Physics2D.OverlapBox(transform.position, size, transform.eulerAngles.z, whatIsTarget))
			{
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
