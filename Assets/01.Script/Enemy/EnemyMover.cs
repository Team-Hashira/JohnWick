using Hashira.Entities;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace Hashira.Enemies
{
    public class EnemyMover : EntityMover
    {
        [Header("Ground check setting mk.2")]
        [SerializeField]
        private Vector2 _edgeCheckerSize;
        [SerializeField]
        private Vector2 _wallCheckerSize;
        [SerializeField]
        private Vector2 _edgeCheckerOffset;
        [SerializeField]
        private Vector2 _wallCheckerOffset;

        private EntityRenderer _entityRenderer;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
        }


        public bool IsOnEdge()
            => !Physics2D.OverlapBox(transform.position + new Vector3(_edgeCheckerOffset.x * _entityRenderer.FacingDirection, _edgeCheckerOffset.y), _edgeCheckerSize, 0, _whatIsGround);
        public bool IsWallOnFront()
            => Physics2D.OverlapBox(transform.position + new Vector3(_wallCheckerOffset.x * _entityRenderer.FacingDirection, _wallCheckerOffset.y), _wallCheckerSize, 0, _whatIsGround);

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (_entityRenderer == null) 
                return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + new Vector3(_edgeCheckerOffset.x * _entityRenderer.FacingDirection, _edgeCheckerOffset.y), _edgeCheckerSize);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position + new Vector3(_wallCheckerOffset.x * _entityRenderer.FacingDirection, _wallCheckerOffset.y), _wallCheckerSize);
        }
#endif
    }
}