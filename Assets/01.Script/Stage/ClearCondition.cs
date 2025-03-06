using Hashira.Enemies;
using Hashira.Entities;
using UnityEngine;
using UnityEngine.Events;
using Hashira.Stage.Area;

namespace Hashira.Stage
{
	public abstract class ClearCondition
	{
		public abstract void Init();

		[HideInInspector] public bool isClear = false;
		public UnityEvent ClearEvent;
	}

	[System.Serializable]
	public class EnemyClearCondition : ClearCondition
	{
		public Enemy[] enemies;
		private int _enemyCount = 0;

		public override void Init()
		{
			_enemyCount = enemies.Length;

			foreach (Enemy enemy in enemies)
			{
				enemy.GetEntityComponent<EntityHealth>().OnDieEvent += HandleEnemyCounting;
			}
		}

		private void HandleEnemyCounting()
		{
			--_enemyCount;
			if (_enemyCount <= 0)
				ClearEvent?.Invoke();

            for (int i = 0; i < enemies.Length; i++)
                enemies[i].gameObject.SetActive(true);
		}
	}

	[System.Serializable]
	public class AreaClearCondition : ClearCondition
	{
		public Area.Area area;

		public override void Init()
		{
			area.ClearEvent.AddListener(HandleClear);
		}

		private void HandleClear()
		{
			area.ClearEvent.AddListener(HandleClear);
			ClearEvent?.Invoke();
		}
	}

	[System.Serializable]
	public class OtherClearCondition : ClearCondition
	{
		public override void Init()
		{
		}
	}
}
