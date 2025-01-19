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
        private Vector2  _wallCheckerSize;
        [SerializeField]
        private Vector2 _edgeCheckerOffset;
        [SerializeField]
        private Vector2 _wallCheckerOffset;

        public bool IsOnEdge()
            => !Physics2D.OverlapBox((Vector2)transform.position + _edgeCheckerOffset, _edgeCheckerSize, 0, _whatIsGround);
        public bool IsWallOnFront()
            => Physics2D.OverlapBox((Vector2)transform.position + _wallCheckerOffset, _wallCheckerSize, 0, _whatIsGround);

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube((Vector2)transform.position + _edgeCheckerOffset, _edgeCheckerSize);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube((Vector2)transform.position + _wallCheckerOffset, _wallCheckerSize);
        }
#endif
    }
}
