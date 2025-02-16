using UnityEngine;

namespace Hashira.Stage.Area
{
    public class ObjectTriggerArea : TriggerArea
	{
		public void FixedUpdate()
		{
			if (Physics2D.OverlapBox(transform.position, size, transform.eulerAngles.z, whatIsTarget))
			{
				Invoke();
			}
		}
	}
}
