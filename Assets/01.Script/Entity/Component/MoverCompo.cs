using Doryu.StatSystem;
using System;
using UnityEngine;

public class MoverCompo : MonoBehaviour, IEntityComponent
{
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

    [Header("Ground check setting")]
    [SerializeField] private Vector2 _groundCheckerSize;
    [SerializeField] private LayerMask _whatIsGround;

    private Entity _entity;

    private float _xVelocity;

    public bool IsGround { get; private set; }

    public void Initialize(Entity entity)
    {
        _entity = entity;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        ApplyMovement();
    }

    private void GroundCheck()
    {
        Collider2D[] raycastHit2Ds = Physics2D.OverlapBoxAll((Vector2)transform.position, _groundCheckerSize, 0, _whatIsGround);

        IsGround = raycastHit2Ds.Length > 0;
    }

    private void ApplyMovement()
    {
        Rigidbody2D.linearVelocityX = _xVelocity;
    }

    public void SetMovement(float xMovement)
    {
        _xVelocity = xMovement;
    }

    public void Jump(float jumpPower)
    {
        if (IsGround)
            Rigidbody2D.linearVelocityY = jumpPower;
    }

    public void StopImmediant(bool withYVelocity = false)
    {
        _xVelocity = 0;
        Rigidbody2D.linearVelocityX = 0;

        if (withYVelocity)
            Rigidbody2D.linearVelocityY = 0;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _groundCheckerSize);
    }
#endif
}
