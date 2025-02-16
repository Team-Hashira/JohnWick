using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Stage.Area
{
    public abstract class TriggerArea : MonoBehaviour
    {
		public UnityEvent Event;

		public LayerMask whatIsTarget;
		public Vector2 size = Vector2.one;

		public bool isOnlyOnce = true;
		protected bool isTrigged = false;

		[SerializeField] protected Color _gizmosColor = Color.white;

		protected void Invoke()
		{
			isTrigged = true;
			if (isOnlyOnce && isTrigged) return;

			Event?.Invoke();
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = _gizmosColor;
			Gizmos.DrawWireCube(transform.position, size);
			Gizmos.color = Color.white;
		}
	}
}
