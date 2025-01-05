using Hashira.Entities;
using UnityEngine;

public interface IDamageable
{
	public EEntityPartType ApplyDamage(int value, Collider2D collider2);
}

public interface IRecoverable
{
	public void ApplyRecovery(int value);
}
