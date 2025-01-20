using Unity.Cinemachine;
using UnityEngine;

namespace Hashira.Combat
{
    public class ConfineCollider : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D _confineCollider;
        [SerializeField] private BoxCollider2D _sideColliderR;
        [SerializeField] private BoxCollider2D _sideColliderL;
        private CinemachineConfiner2D _cinemachineConfiner;
        
        private void SetConfine(Vector2 min, Vector2 max)
        {
            _confineCollider.points = new Vector2[]
            {
                new(min.x, max.y),
                new(min.x, min.y),
                new(max.x, min.y),
                new(max.x, max.y)
            };
            
            _sideColliderR.size = new Vector2(1, max.y*2);
            _sideColliderL.size = new Vector2(1, max.y*2);
            
            _sideColliderR.offset = new Vector2(min.x-1, 0);
            _sideColliderL.offset = new Vector2(max.x+1, 0);
            
            _cinemachineConfiner ??= FindFirstObjectByType<CinemachineConfiner2D>();
            _cinemachineConfiner.BoundingShape2D = _confineCollider;
        }
    }
}
