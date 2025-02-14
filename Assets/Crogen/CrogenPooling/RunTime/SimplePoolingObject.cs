using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Crogen.CrogenPooling;

public class SimplePoolingObject : MonoBehaviour, IPoolingObject
{
	string IPoolingObject.OriginPoolType { get; set; }
	GameObject IPoolingObject.gameObject { get; set; }

	[Header("Life")]
	public bool isAutoPush = true;
	public float duration;
	public float CurLifetime { get; private set; } = 0;

	[Header("Events")]
	public UnityEvent popEvent;
	public UnityEvent pushEvent;

	public void OnPop()
    {
        popEvent?.Invoke();
	}

	public void OnPush()
	{
		pushEvent?.Invoke();
		CurLifetime = 0f;
	}

	private void Update()
	{
		if (isAutoPush)
        {
            CurLifetime += Time.deltaTime;
            if (CurLifetime > duration)
            {
                this.Push();
            }
        }
	}
}
