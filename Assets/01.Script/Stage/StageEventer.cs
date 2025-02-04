using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Stage
{
	public class StageEventer : MonoBehaviour
	{
		[SerializeField] private List<EnemyClearCondition> _enemyClearConditionalList;
		[SerializeField] private List<AreaClearCondition> _areaClearConditionalList;
		[SerializeField] private OtherClearCondition _otherClearConditional;

		private void Awake()
		{
			_enemyClearConditionalList.ForEach(condition => condition.Init());
			_areaClearConditionalList.ForEach(condition => condition.Init());
			_otherClearConditional.Init();
		}

		public void InvokeOtherClearConidition()
		{
			_otherClearConditional.ClearEvent?.Invoke();
		}
	}
}
