using Hashira.Entities;
using System;
using UnityEngine;

public interface IDamageable
{
	public EEntityPartType ApplyDamage(int value, RaycastHit2D raycastHit, Transform attackerTrm);
}

public interface IRecoverable
{
	public void ApplyRecovery(int value);
}
