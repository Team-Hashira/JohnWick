using Crogen.AttributeExtension;
using Hashira.Core.EventSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hashira.Pathfind
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
        private List<Node> _nodeList;
        public List<Node> NodeList => _nodeList;
        private Vector3 _offset;

        [Header("Sound Setting")]
        [SerializeField]
        private GameEventChannelSO _soundEventChannel;
        [SerializeField]
        private LayerMask _whatIsGround;

        private void Awake()
        {
            _soundEventChannel.AddListener<SoundGeneratedEvent>(HandleOnSoundGenerated);

        }

        private void HandleOnSoundGenerated(SoundGeneratedEvent evt)
        {
            List<Node> sortedNodeList =
                _nodeList.OrderBy(node => Vector3.Distance(evt.originPosition, node.transform.position)).ToList();
            foreach (Node node in sortedNodeList)
            {
                Vector2 direction = evt.originPosition - node.transform.position;
                float distance = direction.magnitude;
                bool isWallOnBetween = Physics2D.Raycast(node.transform.position, direction.normalized, distance, _whatIsGround);
                if (!isWallOnBetween)
                {
                    var e = SoundEvents.NearbySoundPointEvent;
                    e.node = node;
                    e.loudness = evt.loudness;
                    _soundEventChannel.RaiseEvent(e);
                    return;
                }
            }
            Debug.Log("소리 발생지로부터 가까운 노드를 찾지 못함.");
            var ev = SoundEvents.NearbySoundPointEvent;
            ev.node = sortedNodeList[0];
            ev.loudness = evt.loudness;
            _soundEventChannel.RaiseEvent(ev);
        }

        [Button("Initialize", 20)]
        public void Initialize()
        {
            if (_nodeList.Count() <= 0)
                return;

            foreach (var node in _nodeList)
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
            _nodeList.Clear();
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
                        if (IsStair(_groundTilemap.GetTile(position)))
                        {
                            CreateNode(NodeType.Stair, fixedPos);
                            continue;
                        }
                        if (IsStairOnLeftOrRight(position))
                        {
                            CreateNode(NodeType.StairEnter, fixedPos);
                            continue;
                        }
                        if (IsWallOnLeftOrRight(fixedPos))
                        {
                            CreateNode(NodeType.Ground, fixedPos);
                            continue;
                        }
                        if (IsEmptyOnLeftOrRight(position))
                        {
                            CreateNode(NodeType.Ground, fixedPos);
                            continue;
                        }
                        CreateNode(NodeType.Ground, fixedPos);
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
                        CreateNode(NodeType.OneWay, fixedPos);
                    }
                }
            }
        }

        [Button("Connect Nodes")]
        public void ConnectNodes()
        {
            _nodeList.ForEach(node => node.SetupConnection(1.5f));
        }

        private void CreateNode(NodeType type, Vector3 position)
        {
            Node node = Instantiate(_nodePrefab, transform);
            node.Initialize(this, position + _offset, type);
            _nodeList.Add(node);
        }

        private bool IsStairOnLeftOrRight(Vector3Int position)
        {
            Vector2Int[] dirs = { Vector2Int.left, Vector2Int.right };
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
            Debug.Log(position);
            return IsStair(tile);
        }

        private bool IsStair(TileBase tile)
        {
            for (int i = 0; i < _stairTileBases.Length; i++)
            {
                if (tile == _stairTileBases[i])
                    return true;
            }
            return false;
        }
    }
}
