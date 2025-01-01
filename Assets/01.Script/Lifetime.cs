using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Lifetime : MonoBehaviour
{
    public float lifetime;
    [SerializeField] private float _dieDelay;
    protected bool _isDead;

    public void Spawn()
    {
        _isDead = false;
        StartCoroutine(DieCoroutine(lifetime));
    }

    private IEnumerator DieCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Die();
    }
    private IEnumerator DelayDieCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        DelayDie();
    }

    public virtual void Die()
    {
        if (_isDead) return;
        _isDead = true;
        StartCoroutine(DelayDieCoroutine(_dieDelay));
    }
    public abstract void DelayDie();
}
