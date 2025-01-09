using Crogen.AttributeExtension;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hashira.Pathfinder
{
    public class NodeGenerator : MonoBehaviour
    {
        [SerializeField]
        private Node _nodePrefab;

        [SerializeField]
        private Tilemap _groundTilemap, _oneWayTilemap;

        [SerializeField]
        private TileBase[] _stairTileBases;

        [SerializeField]
        private List<Node> _nodes;

        private Vector3 _offset;
        

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
            _nodes.Clear();
        }

        [Button("Generate Nodes", 10)]
        public void GenerateNodes()
        {
            Initialize();
            _offset = new Vector2(0.5f, -0.5f) + -(Vector2)_groundTilemap.transform.position;
            BoundsInt groundBounds = _groundTilemap.cellBounds;
            BoundsInt oneWayBounds = _oneWayTilemap.cellBounds;

            for (int x = groundBounds.xMin; x <= groundBounds.xMax; x++)
            {
                for (int y = groundBounds.yMin; y <= groundBounds.yMax; y++)
                {
                    Vector3Int position = new Vector3Int(x, y);
                    if (_groundTilemap.HasTile(position))
                    {
                        Vector3Int fixedPos = position + Vector3Int.up;
                        if (_groundTilemap.HasTile(fixedPos)) //�ٷ� ���� Ÿ�ϸ��� �ִ°�? (õ���̰ų�, ���̰ų�, ������ �巯�� ���� ���� Ÿ����)
                            continue;
                        CreateNode(NodeType.Curve, fixedPos);
                        if (IsStair(_groundTilemap.GetTile(position))) //����̸� ��������.
                            continue;
                        if (IsWallOnLeftOrRight(fixedPos))
                        {
                            CreateNode(NodeType.Curve, fixedPos);
                            continue;
                        }
                        if (IsEmptyOnLeftOrRight(position))
                        {
                            CreateNode(NodeType.Curve, fixedPos);
                            continue;
                        }
                        if (IsStairOnLeftOrRight(position))
                        {
                            CreateNode(NodeType.Curve, fixedPos);
                            continue;
                        }
                    }
                }
            }

            for (int x = oneWayBounds.xMin; x <= oneWayBounds.xMax; x++)
            {
                for (int y = oneWayBounds.yMin; y <= oneWayBounds.yMax; y++)
                {
                    Vector3Int position = new Vector3Int(x, y);
                    if (_oneWayTilemap.HasTile(position))
                    {
                        Vector3Int fixedPos = position + Vector3Int.up;
                        if (_oneWayTilemap.HasTile(fixedPos)) //�ٷ� ���� Ÿ�ϸ��� �ִ°�? (������ Ÿ�ϸ��� 2����)
                            continue;
                        CreateNode(NodeType.Curve, fixedPos);
                    }
                }
            }
        }

        [Button("Connect Nodes")]
        public void ConnectNodes()
        {

        }

        private void CreateNode(NodeType type, Vector3 position)
        {
            Node node = Instantiate(_nodePrefab, transform);
            node.transform.position = position + _offset;
            node.position = position;
            node.nodeType = type;
            _nodes.Add(node);
        }

        private bool IsStairOnLeftOrRight(Vector3Int position)
        {
            Vector2Int[] dirs = Direction2D.GetDirections(DirectionType.Left, DirectionType.Right); //�˻� �������κ��� ���ʸ� Ž��.
            foreach (var dir in dirs)
            {
                if (IsStair(position + (Vector3Int)dir)) // �ٷ� ���� ����� �ִ��� üũ.
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsEmptyOnLeftOrRight(Vector3Int posiiton)
        {
            Vector2Int[] dirs = Direction2D.GetDirections(DirectionType.Left, DirectionType.Right); //�˻� �������κ��� ���ʸ� Ž��.
            foreach (var dir in dirs) // ����ִ��� üŷ.
            {
                if (!_groundTilemap.HasTile(posiiton + (Vector3Int)dir)) // �ٷ� ���� ����ִ��� üũ.
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsWallOnLeftOrRight(Vector3Int posiiton)
        {
            Vector2Int[] wallDirs = Direction2D.GetDirections(DirectionType.Left, DirectionType.Right); //�˻� �������κ��� ���ʸ� Ž��.
            foreach (var dir in wallDirs) // �� Ÿ������ üŷ.
            {
                if (_groundTilemap.HasTile(posiiton + (Vector3Int)dir)) // �ٷ� ���� ���� �ִ��� üũ.
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsStair(Vector3Int position)
        {
            TileBase tile = _groundTilemap.GetTile(position);
            if (tile == null)
                return false;
            foreach (var t in _stairTileBases)
            {
                if (tile == t)
                    return true;
            }
            return false;
        }

        private bool IsStair(TileBase tile)
        {
            foreach(var t in _stairTileBases)
            {
                if (tile == t)
                    return true;
            }
            return false;
        }
    }
}
