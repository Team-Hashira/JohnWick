using Crogen.AttributeExtension;
using Hashira.Entities;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Stage.Area
{
    public class BattleArea : Area
	{
		[SerializeField] private GameObject _rObject;
		[SerializeField] private GameObject _lObject;

		[SerializeField] private GameObject _rEnemyOnlyCollider;
		[SerializeField] private GameObject _lEnemyOnlyCollider;

		[SerializeField] private UnityEvent BattleStartEvent;
		[SerializeField] private List<EntityHealth> _entityList;

		[SerializeField] private Vector2 _battleSize = new Vector2(17.78f, 10.05f);
		[SerializeField] private Vector2 _cameraMovableSize = new Vector2(17.78f, 10.05f);
		private int _enemyCount = 0;


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
			var polygonCollider = GetComponent<PolygonCollider2D>();
			_cam ??= GetComponentInChildren<CinemachineCamera>();

			Debug.Assert(polygonCollider != null, "PolygonCollider is null!");

			Vector2[] sizes = new Vector2[4];

			sizes[0] = _cameraMovableSize;
			sizes[1] = new Vector2(-_cameraMovableSize.x, _cameraMovableSize.y);
			sizes[2] = -_cameraMovableSize;
			sizes[3] = new Vector2(_cameraMovableSize.x, -_cameraMovableSize.y);

			polygonCollider.SetPath(0, sizes);

			Transform r = _rObject.transform;
			Transform l = _lObject.transform;

			var colls = GetComponentsInChildren<BoxCollider2D>();
			for (int i = 0; i < colls.Length; i++)
				colls[i].size = new Vector2(1f, _battleSize.y * 2);

			if (r != null)
				r.localPosition = new Vector3(_battleSize.x + 0.5f, 0, 0);
			if (l != null)
				l.localPosition = new Vector3(-_battleSize.x - 0.5f, 0, 0);

			r = _rEnemyOnlyCollider.transform;
			l = _lEnemyOnlyCollider.transform;

			if (r != null)
				r.localPosition = new Vector3(_battleSize.x + 0.5f, 0, 0);
			if (l != null)
				l.localPosition = new Vector3(-_battleSize.x - 0.5f, 0, 0);

			_cam.GetComponent<CinemachineConfiner2D>().InvalidateBoundingShapeCache();
			EditorUtility.SetDirty(polygonCollider);
		}
#endif

		private void Start()
		{
			_cameraManager = CameraManager.Instance;
			_cam.Follow = FindFirstObjectByType<InternallyDividedPosition>().transform;

			_enemyCount = _entityList.Count;
			for (int i = 0; i < _enemyCount; i++)
				_entityList[i].OnDieEvent += HandleCounting;
		}

		public void StartBattle()
		{
			_rObject?.SetActive(true);
			_lObject?.SetActive(true);
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
			_rObject?.SetActive(false);
			_lObject?.SetActive(false);
			ClearEvent?.Invoke();
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
