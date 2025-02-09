using Crogen.CrogenPooling;
using Hashira.Entities;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Hashira.Projectile
{
    public class Bullet : PushLifetime
    {
        [SerializeField] private ProjectileCollider2D _projectileCollider;
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private EffectPoolType _hitEffect;
        [SerializeField] private EffectPoolType _spakleEffect;
        [SerializeField] private EffectPoolType _bloodEffect;
        [SerializeField] private EffectPoolType _bloodBackEffect;
        [SerializeField] private EffectPoolType _wallBloodEffect;
        private float _speed;
        private int _damage;
        private int _penetration;
        private int _currentPenetration;
        private LayerMask _whatIsTarget;
        private SpriteRenderer _spriteRenderer;

        private List<Collider2D> _penetratedColliderList = new List<Collider2D>();

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (_isDead) return;

            Vector3 movement = transform.right * Time.fixedDeltaTime * _speed;
            bool isHit = _projectileCollider.CheckCollision(_whatIsTarget, out RaycastHit2D[] hits, movement);
            RaycastHit2D hit = hits.ToList().FirstOrDefault(hit => _penetratedColliderList.Contains(hit.collider) == false);
            if (isHit && hit != default)
            {
                //Move
                transform.position += transform.right * hit.distance;

                //Damage
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    int damage = CalculatePenetration(_damage, _penetration - _currentPenetration);
                    EEntityPartType parts = damageable.ApplyDamage(damage, hit, transform, 4f);

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

                // Penetration
                if (hit.transform.TryGetComponent(out IPenetrable penetrable))
                {
                    _currentPenetration -= penetrable.Resistivity;
                    if (_currentPenetration > 0)
                    {

                        _penetratedColliderList.Add(hit.collider);
                    }
                    else
                        Die();
                }
                else
                    Die();

            }
            else
                transform.position += movement;
        }

        public int CalculatePenetration(float damage, int penetratedCount)
        {
            if (penetratedCount == 0) return Mathf.CeilToInt(damage);
            return CalculatePenetration(damage * 0.8f, penetratedCount - 1);
        }

        public void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration)
        {
            _damage = damage;
            _speed = speed;
            _penetration = penetration;
            _currentPenetration = penetration;
            _whatIsTarget = whatIsTarget;
            transform.right = direction;
            _spriteRenderer.enabled = true;
            _trailRenderer.enabled = true;
            _penetratedColliderList = new List<Collider2D>();
        }

        public override void Die()
        {
            base.Die();
            _spriteRenderer.enabled = false;
        }

        public override void DelayDie()
        {
            base.DelayDie();
            _trailRenderer.enabled = false;
        }
    }
}