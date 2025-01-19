using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Pathfind
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
        public NodeType NodeType { get; set; }
        public List<Node> Neighbors { get; private set; } = new List<Node>();

        public void SetupConnection(float nodeSpace)
        {
            Neighbors.Clear();
            LayerMask layer = LayerMask.GetMask("Node");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, nodeSpace, layer);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject == gameObject)
                    continue;
                Node targetNode = collider.GetComponent<Node>();
                if (NodeType == NodeType.Stair)
                {
                    if (targetNode.NodeType != NodeType.Stair && targetNode.NodeType != NodeType.StairEnter)
                        continue;
                }
                if (NodeType == NodeType.OneWay)
                {
                    if (targetNode.NodeType == NodeType.Stair)
                        continue;
                }
                Neighbors.Add(targetNode);
            }
        }
    }
}
