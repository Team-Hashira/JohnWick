using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Pathfinder
{
    public enum NodeType
    {
        Ground,
        OneWay,
        Stair,
        StairEnter,
    }

    public class Node : MonoBehaviour
    {
        public Vector2 position;
        public NodeType nodeType;
        public List<Node> neighbors = new List<Node>();

        public void SetupConnection(float nodeSpace)
        {
            neighbors.Clear();
            LayerMask layer = LayerMask.GetMask("Node");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, nodeSpace, layer);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject == gameObject)
                    continue;
                Node targetNode = collider.GetComponent<Node>();
                if (nodeType == NodeType.Stair)
                {
                    if (targetNode.nodeType != NodeType.Stair && targetNode.nodeType != NodeType.StairEnter)
                        continue;
                }
                if (nodeType == NodeType.OneWay)
                {
                    if (targetNode.nodeType == NodeType.Stair)
                        continue;
                }
                neighbors.Add(targetNode);
            }
        }
    }
}
