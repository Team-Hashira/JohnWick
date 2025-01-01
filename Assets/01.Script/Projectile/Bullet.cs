using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : DestroyLifetime
{
    [SerializeField] private ProjectileCollider2D _projectileCollider;
    [SerializeField] private Transform _hitEffect;
    [SerializeField] private Transform _spakleEffect;
    [SerializeField] private Transform _bloodEffect;
    private float _speed;
    private int _damage;
    private LayerMask _whatIsTarget;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

            //Damage
            int damage = _damage;
            if (hit.transform.TryGetComponent(out Entity entity))
            {
                bool isHeadShot = false;
                if (entity.TryGetEntityComponent(out PartsColliderCompo partsColliderCompo))
                {
                    EEntityParts parts = partsColliderCompo.Hit(hit.collider);
                    Debug.Log("PartsColliderCheck : " + parts.ToString());
                    isHeadShot = parts == EEntityParts.Head;
                }
                if (entity.TryGetEntityComponent(out HealthCompo health))
                {
                    if (isHeadShot)
                    {
                        //Effect
                        Transform headBloodEffect = Instantiate(_bloodEffect, hit.point, Quaternion.identity);
                        headBloodEffect.up = hit.normal;
                    }
                    health.ApplyDamage(isHeadShot ? damage * 5 : damage);

                    //Effect
                    Transform bloodEffect = Instantiate(_bloodEffect, hit.point, Quaternion.identity);
                    bloodEffect.up = hit.normal;
                    Transform hitEffect = Instantiate(_hitEffect, hit.point + hit.normal * 0.1f, Quaternion.identity);
                    hitEffect.up = -hit.normal;
                }
            }
            else
            {
                //Effect
                Transform spakleEffect = Instantiate(_spakleEffect, hit.point + hit.normal * 0.1f, Quaternion.identity);
                spakleEffect.up = hit.normal;
            }

            //Die
            Die();
        }
        else
            transform.position += movement;
    }

    public void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage)
    {
        _damage = damage;
        _speed = speed;
        _whatIsTarget = whatIsTarget;
        transform.right = direction;
        Spawn();
    }

    public override void Die()
    {
        base.Die();
        _spriteRenderer.enabled = false;
    }
}
