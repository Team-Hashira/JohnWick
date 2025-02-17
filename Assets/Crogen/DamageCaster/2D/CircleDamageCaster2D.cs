using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDamageCaster2D : DamageCaster2D
{
    [Header("BoxCaster2D")]
    public Vector2 center;
    public bool isLocalScale = true;
    public float radius = 1f;

    private float GetScaledSize(float radius)
    {
        float finalRadius;
        if (isLocalScale)
            finalRadius = Mathf.Max(transform.lossyScale.x * radius, transform.lossyScale.y * radius);
        else
            finalRadius = radius;


        return finalRadius;
    }
    
    public override RaycastHit2D[] CastOverlap(Vector2 moveTo = default)
    {
        _raycastHits = Physics2D.CircleCastAll(transform.position + transform.rotation * GetFinalCenter(center),
            GetScaledSize(radius), moveTo.normalized, moveTo.magnitude, _whatIsCastable);
        return _raycastHits;
    }
    
    private void OnDrawGizmos()
    {
        if (excluded) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireSphere(GetFinalCenter(center), GetScaledSize(radius));
        Gizmos.matrix = oldMatrix;
        Gizmos.color = Color.white;
    }
}
