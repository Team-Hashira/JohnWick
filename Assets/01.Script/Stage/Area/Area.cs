using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Stage.Area
{
    public abstract class Area : MonoBehaviour
    {
		public UnityEvent ClearEvent;
		public UnityEvent PlayerEnterEvent;
		public UnityEvent PlayerExitEvent;
	}
}
