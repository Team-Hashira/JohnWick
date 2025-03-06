using Hashira.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Stage
{
	public class StageEventer : MonoBehaviour
	{
        [SerializeField] private Transform _startPlayerPosTrm;


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

            for (int i = 0; i < _enemyClearConditionalList[0].enemies.Length; i++)
                _enemyClearConditionalList[0].enemies[i].gameObject.SetActive(true);

            for (int i = 1; i < _enemyClearConditionalList.Count; i++)
                for (int j = 0; j < _enemyClearConditionalList[i].enemies.Length; j++)
                    _enemyClearConditionalList[i].enemies[j].gameObject.SetActive(false);
                

            PlayerManager.Instance.Player.transform.position = _startPlayerPosTrm.position;
        }
        
        private void StageAllClear()
        {
            AllClearEvent?.Invoke();
        }
	}
}
