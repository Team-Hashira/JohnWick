using Hashira.Core.StatSystem;
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
        protected RaycastHit2D _hitedGround;
        protected float _gravity;
        protected float _gravityScale;

        protected Entity _entity;

        protected float _yMovement;
        protected float _xMovement;
        protected Vector2 _velocity;

        public bool IsGrounded { get; private set; }

        public virtual void Initialize(Entity entity)
        {
            _gravityScale = Rigidbody2D.gravityScale;
            Rigidbody2D.gravityScale = 0;
            _gravity = Physics2D.gravity.y;
            _yMovement = 0;
            _xMovement = 0;
            _entity = entity;
        }

        private void FixedUpdate()
        {
            GroundCheck();
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

        private void GroundCheck()
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2)transform.position,
                _groundCheckerSize, 0, Vector2.down, _downDistance, _whatIsGround);

            IsGrounded = hits.Length > 0 && _yMovement < 0;

            if (IsGrounded)
                _hitedGround = hits[0];
        }

        private void ApplyVelocity()
        {
            //기본 움직임 조작
            if (IsGrounded)
            {
                //바닥의 기욱기에 따라 힘의 방향 회전
                _velocity = Vector3.ProjectOnPlane(Vector2.right, _hitedGround.normal).normalized * _xMovement;
                _velocity += _hitedGround.normal * _yMovement;
            }
            else
                _velocity = new Vector2(_xMovement, _yMovement);

            //대입
            Rigidbody2D.linearVelocity = _velocity;
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