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

		private CinemachineCamera _cam;

		private void Awake()
		{
			_cam = GetComponentInChildren<CinemachineCamera>();
			_cam.Priority = -1;
		}

		private void Start()
		{
			_cam.Follow = FindFirstObjectByType<InternallyDividedPosition>().transform;

			_enemyCount = _entityList.Count;
			foreach (var entity in _entityList)
				entity.OnDieEvent += HandleCounting;
		}

		private void OnValidate()
		{
			PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();

			Transform r = transform.Find("R");
			Transform l = transform.Find("L");

			if (r != null)
				r.localPosition = new Vector3(polygonCollider.GetPath(0)[0].x+0.5f, 0, 0);
			if (l != null) 
				l.localPosition = new Vector3(polygonCollider.GetPath(0)[1].x-0.5f, 0, 0);
		}

		public void StartBattle()
		{
			BattleStartEvent?.Invoke();
		}

		private void HandleCounting()
		{
			--_enemyCount;
			Debug.Log("Á×¾ú´Ù´Ï±î?");
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
				if(_enemyCount > 0)
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
