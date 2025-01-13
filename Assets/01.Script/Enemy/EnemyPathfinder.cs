using Hashira.Entities;
using Hashira.Pathfind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public float StopDistance { get; set; } = 1f;

        public void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
            _entityMover = entity.GetEntityComponent<EntityMover>();
        }

        public void PathfindAndMove(Node targetNode)
        {
            _currentPath = _pathfinder.FindPath(_entityMover.CurrentNode, targetNode);
            if (_moveCoroutine == null)
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
                float x = -currentNode.transform.position.x - transform.position.x;
                while (true)
                {
                    _entityMover.SetMovement(Mathf.Sign(x));
                    if(Mathf.Abs(x) <= StopDistance)
                    {
                        break;
                    }
                    yield return null;
                }
            }
        }
    }
}
