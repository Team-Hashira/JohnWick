using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Pathfinder
{
    public enum NodeType
    {
        Common,
        Curve,
    }

    public class Node : MonoBehaviour
    {
        public Vector2 position;
        public NodeType nodeType;
        public List<Node> neighbors = new List<Node>();
    }
}
