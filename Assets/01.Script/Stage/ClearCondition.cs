using Hashira.Enemies;
using Hashira.Entities;
using UnityEngine;
using UnityEngine.Events;

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
		}
	}

	[System.Serializable]
	public class AreaClearCondition : ClearCondition
	{
		public BattleArea battleArea;

		public override void Init()
		{
			battleArea.ClearEvent += HandleClear;
		}

		private void HandleClear()
		{
			battleArea.ClearEvent -= HandleClear;
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
