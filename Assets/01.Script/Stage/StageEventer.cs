using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Stage
{
	public class StageEventer : MonoBehaviour
	{
        public UnityEvent AllClearEvent;

		[SerializeField] private List<EnemyClearCondition> _enemyClearConditionalList;

		private void Start()
		{
			_enemyClearConditionalList.ForEach(condition => condition.Init());

            for (int i = 0; i < _enemyClearConditionalList.Count-1; i++)
            {
                _enemyClearConditionalList[i].ClearEvent.AddListener(() => _enemyClearConditionalList[i+1].ShowAllEnemies());
            }


            _enemyClearConditionalList[_enemyClearConditionalList.Count - 1].ClearEvent.AddListener(StageAllClear);
		}

        private void StageAllClear()
        {
            AllClearEvent?.Invoke();
        }
	}
}
