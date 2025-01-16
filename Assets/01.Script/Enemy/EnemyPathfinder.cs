using Crogen.AttributeExtension;
using Hashira.Entities;
using Hashira.Pathfind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHierarchy.Libs;

namespace Hashira.Enemies
{
    [RequireComponent(typeof(Pathfinder))]
    [RequireComponent(typeof(EntityMover))]
    public class EnemyPathfinder : MonoBehaviour, IEntityComponent
    {
        private Enemy _enemy;
        private EntityMover _entityMover;
        private Pathfinder _pathfinder;

        private List<Node> _currentPath;
        private Coroutine _moveCoroutine;
        [SerializeField]
        private LayerMask _onewayPlatform;
        [field: SerializeField]
        public float StopDistance { get; set; } = 1f;

        [SerializeField]
        private Node _targetNode;

        public void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
            _entityMover = entity.GetEntityComponent<EntityMover>();
            _pathfinder = GetComponent<Pathfinder>();
        }

        [Button("Test")]
        private void Test()
        {
            PathfindAndMove(_targetNode);
        }

        public void PathfindAndMove(Node targetNode)
        {
            _currentPath = _pathfinder.FindPath(_entityMover.CurrentNode, targetNode);
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
            StopCoroutine(_moveCoroutine);
            _entityMover.StopImmediately();
        }

        private IEnumerator MoveCoroutine()
        {
            for (int i = 0; i < _currentPath.Count; i++)
            {
                Node currentNode = _currentPath[i];
                float x = currentNode.transform.position.x - transform.position.x;
                bool hasToJump = currentNode.nodeType == NodeType.Stair;
                //if(hasToJump)
                //    _enemy.GetComponent<Collider2D>().excludeLayers = _onewayPlatform;
                //else
                //    _enemy.GetComponent<Collider2D>().excludeLayers = 0;
                _entityMover.UnderJump(hasToJump);
                while (true)
                {
                    _entityMover.SetMovement(Mathf.Sign(x) * 10);
                    float distance = currentNode.transform.position.x - transform.position.x;
                    if (Mathf.Abs(distance) <= StopDistance)
                    {
                        break;
                    }
                    yield return null;
                }
            }
        }
    }
}
