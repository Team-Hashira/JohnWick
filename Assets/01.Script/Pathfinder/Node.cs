using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Pathfinder
{
    public enum NodeType
    {
        Ground,
        OneWayPlatform,
        Stair,
    }

    public class Node : MonoBehaviour
    {
        public Vector2 position;
        public NodeType nodeType;
        public List<Node> neighbors = new List<Node>();
    }
}
