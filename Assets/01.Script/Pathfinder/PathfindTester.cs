using Crogen.AttributeExtension;
using UnityEngine;

namespace Hashira.Pathfind
{
    public class PathfindTester : MonoBehaviour
    {
        [SerializeField]
        private Pathfinder _pathfinder;

        [SerializeField]
        private Node _startNode, _targetNode;

        [Button("Pathfinding")]
        private void Pathfind()
        {
            var path = _pathfinder.FindPath(_startNode, _targetNode);
            Node prev = null;
            foreach(var node in path)
            {
                if (prev == null)
                {
                    prev = node;
                    continue;
                }
                Vector3 dir = node.transform.position - prev.transform.position;
                Debug.DrawRay(prev.transform.position + Vector3.up * 0.5f, dir.normalized * dir.magnitude, Color.red, 30);
                prev = node;
            }
        }
    }
}
