using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Stage
{
	public class StageEventer : MonoBehaviour
	{
		[SerializeField] private List<EnemyClearCondition> _enemyClearConditionalList;
		[SerializeField] private List<AreaClearCondition> _areaClearConditionalList;
		[SerializeField] private List<OtherClearCondition> _otherClearConditionalList;

		private void Start()
		{
			_enemyClearConditionalList.ForEach(condition => condition.Init());
			_areaClearConditionalList.ForEach(condition => condition.Init());
			_otherClearConditionalList.ForEach(condition => condition.Init());
		}

		public void InvokeOtherClearConidition(int index)
		{
			_otherClearConditionalList[index].Init();
		}

		public void InvokeAllOtherClearConidition()
		{
			_otherClearConditionalList.ForEach(condition => condition.Init());
		}
	}
}
