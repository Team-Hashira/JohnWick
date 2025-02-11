using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Pathfind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Enemies
{
    [RequireComponent(typeof(Pathfinder))]
    [RequireComponent(typeof(EnemyMover))]
    public class EnemyPathfinder : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent
    {
        private Enemy _enemy;
        private EnemyMover _enemyMover;
        private EntityStat _entityStat;
        private EnemyIgnoreOneway _enemyIgnoreOneway;
        private EntityRenderer _entityRenderer;
        private Pathfinder _pathfinder;

        private List<Node> _currentPath;
        private Coroutine _moveCoroutine;
        [SerializeField]
        private LayerMask _onewayPlatform;

        [SerializeField]
        private StatElementSO _speedElementSO;
        private StatElement _speedElement;

        public event Action OnMoveEndEvent;

        public Node TargetNode { get; private set; }

        public void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
            _currentPath = new List<Node>();
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _enemyIgnoreOneway = entity.GetEntityComponent<EnemyIgnoreOneway>();
            _entityStat = entity.GetEntityComponent<EntityStat>();
            _pathfinder = GetComponent<Pathfinder>();
        }

        public void AfterInit()
        {
            _speedElement = _entityStat.StatDictionary[_speedElementSO];
        }

        //[Button("Test")]
        //private void Test()
        //{
        //    PathfindAndMove(_targetNode);
        //}

        public void PathfindAndMove(Node targetNode)
        {
            _currentPath = _pathfinder.FindPath(_enemyMover.CurrentNode, targetNode);
            TargetNode = targetNode;
#if UNITY_EDITOR
            Node prev = null;
            foreach (var node in _currentPath)
            {
                if (prev == null)
                {
                    prev = node;
                    continue;
                }
                Vector3 dir = node.transform.position - prev.transform.position;
                Debug.DrawRay(prev.transform.position + Vector3.up * 0.5f, dir.normalized * dir.magnitude, Color.red, 100);
                prev = node;
            }
#endif
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }

        public void StopMove()
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
            _enemyMover.StopImmediately();
            Debug.Log("�Ծ�");
        }

        private IEnumerator MoveCoroutine()
        {
            for (int i = 0; i < _currentPath.Count; i++)
            {
                if (i == 0)
                    continue;
                Node currentNode = _currentPath[i];
                float xDiff = Mathf.Sign(currentNode.transform.position.x - transform.position.x);
                bool ignoreOneway = currentNode.NodeType != NodeType.OneWay;
                _enemyMover.SetIgnoreOnewayPlayform(ignoreOneway);
                _enemyIgnoreOneway.SetIgnoreOneway(ignoreOneway);
                if (_entityRenderer.FacingDirection != xDiff)
                    _entityRenderer.Flip();
                while (true)
                {
                    _enemyMover.SetMovement(xDiff * _speedElement.Value);
                    float sign = Mathf.Sign(currentNode.transform.position.x - transform.position.x);
                    if (sign != xDiff)
                    {
                        break;
                    }
                    yield return null;
                }
            }
            OnMoveEndEvent?.Invoke();
        }


    }
}
