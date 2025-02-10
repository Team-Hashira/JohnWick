using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDamageCaster2D : DamageCaster2D
{
	[Header("BoxCaster2D")]
	public Vector2 center;
	public Vector2 size = Vector2.one;

	private Vector2 GetScaledSize(Vector2 size)
	{
		Vector2 finalVec;
		finalVec.x = size.x * transform.lossyScale.x;
		finalVec.y = size.y * transform.lossyScale.y;

		return finalVec;
	}

	public override RaycastHit2D[] CastOverlap(Vector2 moveTo = default)
	{
        _raycastHits = Physics2D.BoxCastAll(transform.position + transform.rotation * GetFinalCenter(center),
			GetScaledSize(size), transform.rotation.z, moveTo.normalized, moveTo.magnitude, _whatIsCastable);
		return _raycastHits;

    }

	private void OnDrawGizmos()
	{
		if (excluded) Gizmos.color = Color.red;
		else Gizmos.color = Color.green;
		Matrix4x4 oldMatrix = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		Gizmos.DrawWireCube(GetFinalCenter(center), GetScaledSize(size));
		Gizmos.matrix = oldMatrix;
		Gizmos.color = Color.white;
	}
}
