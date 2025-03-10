using Hashira.Pathfind;
using UnityEngine;
using System.Linq;

namespace Hashira.Entities
{
    public class EntityMover : MonoBehaviour, IEntityComponent
    {
        [field: SerializeField] public Collider2D Collider2D { get; private set; }
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

        [Header("Ground check setting")]
        [SerializeField] private Vector2 _groundCheckerSize;
        [SerializeField] private float _downDistance;
        [SerializeField] protected LayerMask _whatIsGround;
        [SerializeField] private LayerMask _oneWayPlatform;
        protected RaycastHit2D _hitedGround;
        protected bool _gravityActive;
        protected float _gravity;
        protected float _gravityScale;

        protected Entity _entity;

        protected float _yMovement;
        protected float _xMovement;
        public Vector2 Velocity { get; private set; }

        public bool IsGrounded { get; private set; }
        public bool IsOneWayPlatform { get; private set; }
        public bool isManualMove = true;

        [field:SerializeField] public int JumpCount { get; private set; } = 1;
        private int _currentJumpCount = 0;
        [field:SerializeField] public float JumpPower { get; private set; }

		[Header("Node Set")]
        [SerializeField]
        private LayerMask _whatIsNode;
        public Node CurrentNode { get; private set; }

        public virtual void Initialize(Entity entity)
        {
            _gravityScale = Rigidbody2D.gravityScale;
            Rigidbody2D.gravityScale = 0;
            _gravity = Physics2D.gravity.y;
            _yMovement = 0;
            _xMovement = 0;
            _entity = entity;
            isManualMove = true;
            _gravityActive = true;
            _whatIsGround |= _oneWayPlatform;
        }

        protected virtual void FixedUpdate()
        {
            GroundAndNodeCheck();
            ApplyGravity();
            ApplyVelocity();
        }

        public void SetGravity(bool active)
        {
            _gravityActive = active;
        }

        private void ApplyGravity()
        {
            if (_gravityActive == false) return;

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
                new Vector2(_groundCheckerSize.x, 10), 0, Vector2.down, 10, _whatIsNode);

            if (groundHits.Length > 0) _hitedGround = groundHits[0];
            IsGrounded = groundHits.Length > 0 && _yMovement < 0;

			if (groundHits.Length > 0) IsOneWayPlatform = groundHits[0].transform.gameObject.GetComponent<PlatformEffector2D>();

            if (IsGrounded) _currentJumpCount = 0;

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

        public void SetMovement(float xMovement, bool isForcedMove = false)
        {
            if (isManualMove == false && isForcedMove == false) return;
            _xMovement = xMovement;
        }

        public void Jump()
        {
            if (isManualMove == false) return;
            if (_currentJumpCount >= JumpCount)
                return;

            _yMovement = JumpPower;

            ++_currentJumpCount;
		}

		public void StopImmediately(bool withYVelocity = false)
        {
            if (isManualMove == false) return;
            _xMovement = 0;
            Rigidbody2D.linearVelocityX = 0;

            if (withYVelocity)
                Rigidbody2D.linearVelocityY = 0;
        }

        public void UnderJump(bool isUnderJump)
        {
            if (IsOneWayPlatform == false) return;
            if (isManualMove == false) return;

            int layerCheck = _whatIsGround & _oneWayPlatform;
            if (layerCheck != 0 && isUnderJump)
                _whatIsGround &= ~(_oneWayPlatform);
            else if (isUnderJump == false)
                _whatIsGround |= _oneWayPlatform;

			Collider2D.isTrigger = isUnderJump;
            Debug.Log(Collider2D.isTrigger);
        }

        public void SetIgnoreOnewayPlayform(bool isIgnore)
        {
            if (isIgnore)
                _whatIsGround &= ~(_oneWayPlatform);
            else
                _whatIsGround |= _oneWayPlatform;
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
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