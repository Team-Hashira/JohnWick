using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private ProjectileCollider2D _projectileCollider;
    [SerializeField] private Transform _hitEffect;
    private float _speed;
    private LayerMask _whatIsTarget;
    private SpriteRenderer _spriteRenderer;
    private bool _isDead = false;

    private float _spawnTime;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (_isDead) return;

        Vector3 movement = transform.right * Time.fixedDeltaTime * _speed;
        if (_projectileCollider.CheckCollision(_whatIsTarget, out RaycastHit2D[] hits, movement))
        {
            transform.position += transform.right * hits[0].distance;
            Transform hitEffect = Instantiate(_hitEffect, hits[0].point + hits[0].normal * 0.1f, Quaternion.identity);
            hitEffect.up = hits[0].normal;
            Die();
        }
        else
            transform.position += movement;

        if (_spawnTime + _lifeTime < Time.time) Die();
    }

    public void Init(LayerMask whatIsTarget, Vector3 direction, float speed)
    {
        _speed = speed;
        _whatIsTarget = whatIsTarget;
        transform.right = direction;
    }

    public void Die()
    {
        if (_isDead) return;
        _isDead = true;
        _spriteRenderer.enabled = false;
        StartCoroutine(DieDelayCoroutine(0.2f));
    }

    public IEnumerator DieDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
