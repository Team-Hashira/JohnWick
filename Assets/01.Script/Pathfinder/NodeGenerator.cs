using Crogen.AttributeExtension;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hashira.Pathfinder
{
    public class NodeGenerator : MonoBehaviour
    {
        [SerializeField]
        private Tilemap _groundTilemap, _oneWayTilemap;

        [SerializeField]
        private Node[] _nodes;

        [Button("Initialize", 20)]
        public void Initialize()
        {
            if (_nodes.Count() <= 0)
                return;

            foreach (var node in _nodes)
            {
                if (node != null)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(node.gameObject);
                    }
                    else
                    {
                        DestroyImmediate(node.gameObject);
                    }
                }
            }
            _nodes = null;
        }

        [Button("Generate Nodes", 10)]
        public void GenerateNodes()
        {
            BoundsInt groundBounds = _groundTilemap.cellBounds;
            BoundsInt oneWayBounds = _oneWayTilemap.cellBounds;

            for(int x = groundBounds.xMin; x <= groundBounds.xMax; x++)
            {
                for(int y = groundBounds.yMin; y <= groundBounds.yMax; y++)
                {
                    Vector3Int position = new Vector3Int(x, y); 
                }
            }
        }

        [Button("Connect Nodes")]
        public void ConnectNodes()
        {

        }


    }
}
