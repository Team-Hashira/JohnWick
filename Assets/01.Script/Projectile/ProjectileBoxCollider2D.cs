using UnityEngine;

public class ProjectileBoxCollider2D : ProjectileCollider2D
{
    public Vector2 size;
    public Vector2 offset;

    public override bool CheckCollision(LayerMask whatIsTarget, out RaycastHit2D[] hits, Vector2 moveTo = default)
    {
        hits = Physics2D.BoxCastAll(transform.position + transform.rotation * offset,
                            size * transform.localScale, transform.rotation.z, moveTo.normalized, moveTo.magnitude, whatIsTarget);

        return hits.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix; 
        Gizmos.DrawWireCube(offset, size);
    }
}
