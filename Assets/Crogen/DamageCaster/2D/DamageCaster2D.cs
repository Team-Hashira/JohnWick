using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Hashira.Entities;
using UnityEditor.VersionControl;

public abstract class DamageCaster2D : MonoBehaviour
{
	public bool excluded;
	public int allocationCount = 32;
	[SerializeField] protected LayerMask _whatIsCastable;
	protected RaycastHit2D[] _raycastHits;
	[SerializeField] private bool _usingExcludeCast = true;
	public List<DamageCaster2D> excludedDamageCasterList;

	public event Action OnCasterEvent;
	public event Action<RaycastHit2D> OnCasterSuccessEvent;
	public event Action<RaycastHit2D> OnDamageCastSuccessEvent;

	protected Vector2 GetFinalCenter(Vector2 center)
	{
		Vector2 finalCenter;
		finalCenter.x = center.x * transform.lossyScale.x;
		finalCenter.y = center.y * transform.lossyScale.y;
		return finalCenter;
	}

	protected virtual void Awake()
	{
		_raycastHits = new RaycastHit2D[allocationCount];
	}

	public void SetLayerMask(LayerMask layerMask)
	{
		_whatIsCastable = layerMask;
    }

	public abstract RaycastHit2D[] CastOverlap(Vector2 moveTo = default);

	public virtual void CastDamage(int damage, Vector2 moveTo = default, Vector2 knockback = default, EAttackType attackType = EAttackType.Default)
	{
		CastOverlap(moveTo);

		//제외
		if (_usingExcludeCast)
			ExcludeCast(_raycastHits);

        HashSet<EntityPartCollider> entityPartColliderSet = new HashSet<EntityPartCollider>();
        //데미지 입히기
        for (int i = 0; i < _raycastHits.Length && i < allocationCount; ++i)
		{
			if (_raycastHits[i].collider == null)
			{
				break;
			}
			else
            {
                OnCasterSuccessEvent?.Invoke(_raycastHits[i]);
            }
            if (_raycastHits[i].transform.TryGetComponent(out IDamageable damageable))
			{
                if (_raycastHits[i].transform.TryGetComponent(out Entity entity) && 
                    entity.TryGetEntityComponent(out EntityPartCollider entityPartCollider) && 
                    entityPartColliderSet.Contains(entityPartCollider) == false)
                    entityPartColliderSet.Add(entityPartCollider);
                else break;


                damageable.ApplyDamage(damage, _raycastHits[i], transform, knockback, attackType);
                OnDamageCastSuccessEvent?.Invoke(_raycastHits[i]);
            }
		}

		OnCasterEvent?.Invoke();
		//이거 내부적으로 메모리를 직접 초기화해서 가벼움
		Array.Clear(_raycastHits, 0, _raycastHits.Length);
	}

	protected void ExcludeCast(RaycastHit2D[] colliders)
	{
		foreach (var excludeCaster in excludedDamageCasterList)
		{
			excludeCaster.CastOverlap();
			colliders = colliders.Except(excludeCaster._raycastHits).ToArray();
		}
	}

	private void OnValidate()
	{
		if (excludedDamageCasterList == null) return;
		for (int i = 0; i < excludedDamageCasterList.Count; ++i)
		{
			if (excludedDamageCasterList[i] == null) continue;

			if (excludedDamageCasterList[i].excluded == false)
				excludedDamageCasterList[i] = null;
		}
	}
}