using Hashira.Entities;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Combat
{
    public class BattleArea : MonoBehaviour
    {
		[SerializeField] private UnityEvent BattleStartEvent;
		[SerializeField] private UnityEvent BattleEndEvent;

		[SerializeField] private List<EntityHealth> _entityList;
		private int _enemyCount = 0;
		private PolygonCollider2D _collider;

		private CinemachineCamera _cam;

		private void Awake()
		{
			_cam = GetComponentInChildren<CinemachineCamera>();
			_cam.Priority = -1;
		}

		private void Start()
		{
			_cam.Follow = FindFirstObjectByType<InternallyDividedPosition>().transform;
		}

		//private void Start()
		//{
		//	Vector2[] paths = _collider.GetPath(0);

		//	for (int i = 0; i < paths.Length; i++)
		//		paths[i] += (Vector2)transform.position;
		//	_collider.SetPath(0, paths);
		//	transform.SetParent(null);
		//	transform.position = Vector3.zero;
		//}

		public void StartBattle()
		{
			_enemyCount = _entityList.Count;

			foreach (var entity in _entityList)
			{
				entity.OnDieEvent += HandleCounting;
			}
			BattleStartEvent?.Invoke();
		}

		private void HandleCounting()
		{
			--_enemyCount;
			if(_enemyCount <= 0)
				EndBattle();
		}

		public void EndBattle()
		{

			BattleEndEvent?.Invoke();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				CameraManager.Instance.ChangeCamera(_cam);
				if(_entityList.Count > 0)
					StartBattle();
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
				CameraManager.Instance.ChangeCamera("Player");
		}
	}
}
