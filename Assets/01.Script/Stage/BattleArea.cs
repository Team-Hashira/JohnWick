using Crogen.AttributeExtension;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Stage
{
    public class BattleArea : MonoBehaviour
    {
		[SerializeField] private GameObject _rObject;
		[SerializeField] private GameObject _lObject;

		[SerializeField] private UnityEvent BattleStartEvent;
		[SerializeField] private UnityEvent BattleEndEvent;
		[SerializeField] private UnityEvent PlayerEnterEvent;
		[SerializeField] private UnityEvent PlayerExitEvent;
		[SerializeField] private List<EntityHealth> _entityList;

		[SerializeField] private Vector2 _size = new Vector2(17.78f, 10.05f);

		private int _enemyCount = 0;

		public event Action ClearEvent;

		private CinemachineCamera _cam;
		private CameraManager _cameraManager;
		private PolygonCollider2D _polygonCollider;

		private void Awake()
		{
			_cam = GetComponentInChildren<CinemachineCamera>();
			_cam.Priority = -1;
		}

#if UNITY_EDITOR
		[Button]
		private void SetSize()
		{
			_polygonCollider ??= GetComponent<PolygonCollider2D>();
			_cam ??= GetComponentInChildren<CinemachineCamera>();

			Vector2[] sizes = new Vector2[4];

			sizes[0] = _size;
			sizes[1] = new Vector2(-_size.x, _size.y);
			sizes[2] = -_size;
			sizes[3] = new Vector2(_size.x, -_size.y);

			_polygonCollider.SetPath(0, sizes);

			Transform r = _rObject.transform;
			Transform l = _lObject.transform;

			var colls = GetComponentsInChildren<BoxCollider2D>();
			foreach (var coll in colls)
				coll.size = new Vector2(1f, _polygonCollider.GetPath(0)[0].y * 2);

			if (r != null)
				r.localPosition = new Vector3(_polygonCollider.GetPath(0)[0].x + 0.5f, 0, 0);
			if (l != null)
				l.localPosition = new Vector3(_polygonCollider.GetPath(0)[1].x - 0.5f, 0, 0);

			_cam.GetComponent<CinemachineConfiner2D>().InvalidateBoundingShapeCache();
			EditorUtility.SetDirty(_polygonCollider);
		}
#endif

		private void Start()
		{
			_cameraManager = CameraManager.Instance;
			_cam.Follow = FindFirstObjectByType<InternallyDividedPosition>().transform;

			_enemyCount = _entityList.Count;
			foreach (var entity in _entityList)
				entity.OnDieEvent += HandleCounting;
		}

		public void StartBattle()
		{
			_rObject?.SetActive(true);
			_lObject?.SetActive(true);
			BattleStartEvent?.Invoke();
			ClearEvent?.Invoke();
		}

		private void HandleCounting()
		{
			--_enemyCount;
			if(_enemyCount <= 0)
				EndBattle();
		}

		public void EndBattle()
		{
			_rObject?.SetActive(false);
			_lObject?.SetActive(false);
			BattleEndEvent?.Invoke();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				_cameraManager?.ChangeCamera(_cam);
				_cameraManager?.ShakeCamera(0, 0, 0);
				PlayerEnterEvent?.Invoke();
				if (_enemyCount > 0)
					StartBattle();
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				_cameraManager?.ChangeCamera("Player");
				_cameraManager?.ShakeCamera(0, 0, 0);
				PlayerExitEvent?.Invoke();
			}
		}
	}
}
