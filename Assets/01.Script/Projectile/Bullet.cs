using Crogen.CrogenPooling;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Projectile
{
    public class Bullet : DestroyLifetime, IPoolingObject
    {
        [SerializeField] private ProjectileCollider2D _projectileCollider;
        [SerializeField] private Transform _hitEffect;
        [SerializeField] private Transform _spakleEffect;
        [SerializeField] private Transform _bloodEffect;
        private float _speed;
        private int _damage;
        private LayerMask _whatIsTarget;
        private SpriteRenderer _spriteRenderer;

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

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
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    EEntityPartType parts = damageable.ApplyDamage(damage, hit.collider);
                    Debug.Log("PartsColliderCheck : " + parts.ToString());
                    if (parts == EEntityPartType.Head)
                    {
                        //Effect
                        Transform headBloodEffect = Instantiate(_bloodEffect, hit.point, Quaternion.identity);
                        headBloodEffect.up = hit.normal;
                    }

                    //Effect
                    Transform bloodEffect = Instantiate(_bloodEffect, hit.point, Quaternion.identity);
                    bloodEffect.up = hit.normal;
                    Transform hitEffect = Instantiate(_hitEffect, hit.point + hit.normal * 0.1f, Quaternion.identity);
                    hitEffect.up = -hit.normal;
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

        public override void DelayDie()
        {
            base.DelayDie();
            
        }

        public void OnPop()
        {

        }

        public void OnPush()
        {

        }
    }
}