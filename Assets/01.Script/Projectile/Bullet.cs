using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private ProjectileCollider2D _projectileCollider;
    [SerializeField] private Transform _hitEffect;
    private float _speed;
    private int _damage;
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
            RaycastHit2D hit = hits[0];

            //Move
            transform.position += transform.right * hit.distance;

            //Effect
            Transform hitEffect = Instantiate(_hitEffect, hit.point + hit.normal * 0.1f, Quaternion.identity);
            hitEffect.up = hit.normal;

            //Damage
            int damage = _damage;
            if (hit.transform.TryGetComponent(out Entity entity))
            {
                if (entity.TryGetEntityComponent(out PartsColliderCompo partsColliderCompo))
                {
                    EEntityParts parts = partsColliderCompo.Hit(hit.collider);
                    Debug.Log("PartsColliderCheck : " + parts.ToString());
                    if (parts == EEntityParts.Head) damage *= 5;
                }
                if (entity.TryGetEntityComponent(out HealthCompo health))
                {
                    health.ApplyDamage(damage);
                    Debug.Log("HealthCheck");
                }
            }


            //Die
            Die();
        }
        else
            transform.position += movement;

        if (_spawnTime + _lifeTime < Time.time) Die();
    }

    public void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage)
    {
        _damage = damage;
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
