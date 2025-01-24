using System.Collections.Generic;
using System.Linq;
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
        private NodeGenerator _nodeGenerator;
        [field:SerializeField]
        public NodeType NodeType { get; private set; }
        [field: SerializeField]
        public List<Node> Neighbors { get; private set; } = new List<Node>();

        public void Initialize(NodeGenerator nodeGenerator, Vector3 position, NodeType type)
        {
            transform.position = position;
            NodeType = type;
            _nodeGenerator = nodeGenerator;
        }

        public void SetupConnection(float nodeSpace)
        {
            Neighbors.Clear();
            LayerMask layer = LayerMask.GetMask("Node");
            List<Node> nodeList = _nodeGenerator.NodeList
                .Where(node => Vector3.Distance(transform.position, node.transform.position) <= nodeSpace)
                .ToList();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, nodeSpace, layer);
            foreach (Node node in nodeList)
            {
                if (node.gameObject == gameObject)
                    continue;
                if (NodeType == NodeType.Stair)
                {
                    if (node.NodeType != NodeType.Stair && node.NodeType != NodeType.StairEnter)
                        continue;
                }
                if (NodeType == NodeType.OneWay)
                {
                    if (node.NodeType == NodeType.Stair)
                        continue;
                }
                Neighbors.Add(node);
            }
        }
    }
}
