using Crogen.CrogenPooling;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Projectile
{
    public class Bullet : PushLifetime
    {
        [SerializeField] private ProjectileCollider2D _projectileCollider;
        [SerializeField] private EffectPoolType _hitEffect;
        [SerializeField] private EffectPoolType _spakleEffect;
        [SerializeField] private EffectPoolType _bloodEffect;
        [SerializeField] private EffectPoolType _bloodBackEffect;
        [SerializeField] private EffectPoolType _wallBloodEffect;
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
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    EEntityPartType parts = damageable.ApplyDamage(damage, hit, transform);

                    if (damageable is EntityHealth health && health.TryGetComponent(out Entity entity))
                    {
                        //Effect
                        ParticleSystem wallBloodEffect = gameObject.Pop(_wallBloodEffect, hit.point, transform.rotation)
                            .gameObject.GetComponent<ParticleSystem>();
                        var limitVelocityOverLifetimeModule = wallBloodEffect.limitVelocityOverLifetime;

                        //Effect
                        ParticleSystem bloodBackEffect = gameObject.Pop(_bloodBackEffect, hit.point, transform.rotation)
                            .gameObject.GetComponent<ParticleSystem>();

                        if (parts == EEntityPartType.Head)
                        {
                            //Effect
                            gameObject.Pop(_bloodEffect, hit.point, Quaternion.LookRotation(Vector3.back, hit.normal));
                            limitVelocityOverLifetimeModule.dampen = 0.6f;
                        }
                        else
                        {
                            limitVelocityOverLifetimeModule.dampen = 0.9f;
                        }
                    }

                    //Effect
                    gameObject.Pop(_hitEffect, hit.point + hit.normal * 0.1f, Quaternion.LookRotation(Vector3.back, -hit.normal));
                }
                else
                {
                    //Effect
                    gameObject.Pop(_spakleEffect, hit.point + hit.normal * 0.1f, Quaternion.LookRotation(Vector3.back, hit.normal));
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
            _spriteRenderer.enabled = true;
        }

        public override void Die()
        {
            base.Die();
            _spriteRenderer.enabled = false;
        }
    }
}