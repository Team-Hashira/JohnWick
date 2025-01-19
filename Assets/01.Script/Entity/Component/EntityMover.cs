using Hashira.Core.StatSystem;
using Hashira.Pathfind;
using System;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityMover : MonoBehaviour, IEntityComponent
    {

        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

        [Header("Ground check setting")]
        [SerializeField] private Vector2 _groundCheckerSize;
        [SerializeField] private float _downDistance;
        [SerializeField] protected LayerMask _whatIsGround;
        [SerializeField] private LayerMask _oneWayPlatform;
        protected RaycastHit2D _hitedGround;
        protected float _gravity;
        protected float _gravityScale;

        protected Entity _entity;

        protected float _yMovement;
        protected float _xMovement;
        public Vector2 Velocity { get; private set; }

        public bool IsGrounded { get; private set; }


        [Header("Node Set")]
        [SerializeField]
        private LayerMask _whatIsNode;
        [field: Space(10)]
        public Node CurrentNode { get; private set; }

        public virtual void Initialize(Entity entity)
        {
            _gravityScale = Rigidbody2D.gravityScale;
            Rigidbody2D.gravityScale = 0;
            _gravity = Physics2D.gravity.y;
            _yMovement = 0;
            _xMovement = 0;
            _entity = entity;
            _whatIsGround += _oneWayPlatform;
        }

        private void FixedUpdate()
        {
            GroundAndNodeCheck();
            ApplyGravity();
            ApplyVelocity();
        }

        private void ApplyGravity()
        {
            if (IsGrounded)
            {
                _yMovement = -3;
            }
            else
            {
                _yMovement += _gravity * Time.fixedDeltaTime * _gravityScale;
            }
        }

        private void GroundAndNodeCheck()
        {
            RaycastHit2D[] groundHits = Physics2D.BoxCastAll((Vector2)transform.position,
                _groundCheckerSize, 0, Vector2.down, _downDistance, _whatIsGround);
            RaycastHit2D[] nodeHits = Physics2D.BoxCastAll((Vector2)transform.position,
                _groundCheckerSize, 0, Vector2.down, _downDistance, _whatIsNode);

            IsGrounded = groundHits.Length > 0 && _yMovement < 0;

            if (nodeHits.Length > 0 && nodeHits[0].collider.TryGetComponent(out Node node)) 
                CurrentNode = node;
        }

        private void ApplyVelocity()
        {
            //기본 움직임 조작
            if (IsGrounded)
            {
                //바닥의 기욱기에 따라 힘의 방향 회전
                Velocity = Vector3.ProjectOnPlane(Vector2.right, _hitedGround.normal).normalized * _xMovement;
                Velocity += _hitedGround.normal * _yMovement;
            }
            else
                Velocity = new Vector2(_xMovement, _yMovement);

            //대입
            Rigidbody2D.linearVelocity = Velocity;
        }

        public void SetMovement(float xMovement)
        {
            _xMovement = xMovement;
        }

        public void Jump(float jumpPower)
        {
            if (IsGrounded)
                _yMovement = jumpPower;
        }

        public void StopImmediately(bool withYVelocity = false)
        {
            _xMovement = 0;
            Rigidbody2D.linearVelocityX = 0;

            if (withYVelocity)
                Rigidbody2D.linearVelocityY = 0;
        }

        public void UnderJump(bool isUnderJump)
        {
            int layerCheck = _whatIsGround & _oneWayPlatform;
            if (layerCheck != 0 && isUnderJump)
                _whatIsGround -= _oneWayPlatform;
            else if (isUnderJump == false)
                _whatIsGround += _oneWayPlatform;

            foreach (var platformEffector in FindObjectsByType<PlatformEffector2D>(FindObjectsSortMode.None))
            {
                platformEffector.rotationalOffset = isUnderJump ? 180 : 0;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _groundCheckerSize);
            Gizmos.color = Color.blue;
            Vector3 targetPos = transform.position + Vector3.down * _downDistance;
            Gizmos.DrawLine(transform.position, targetPos);
            Gizmos.DrawWireCube(targetPos, _groundCheckerSize);
        }
#endif
    }
}