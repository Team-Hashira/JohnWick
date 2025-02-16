using Crogen.CrogenPooling;
using Hashira.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Projectile : PushLifetime
    {
        [SerializeField] protected ProjectileCollider2D _projectileCollider;
        [SerializeField] protected TrailRenderer _trailRenderer;
        [SerializeField] protected EffectPoolType _hitEffect;
        [SerializeField] protected EffectPoolType _spakleEffect;
        protected float _speed;
        protected int _damage;
        protected int _penetration;
        protected int _currentPenetration;
        protected LayerMask _whatIsTarget;
        protected SpriteRenderer _spriteRenderer;
        protected BoxCollider2D _collider;

        protected List<Collider2D> _penetratedColliderList = new List<Collider2D>();

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }

        protected virtual void FixedUpdate()
        {
            if (_isDead) return;

            Vector3 movement = transform.right * Time.fixedDeltaTime * _speed;
            bool isHit = _projectileCollider.CheckCollision(_whatIsTarget, out RaycastHit2D[] hits, movement);
            RaycastHit2D hit = hits.ToList().FirstOrDefault(hit => _penetratedColliderList.Contains(hit.collider) == false);
            if (isHit && hit != default && (hit.transform.TryGetComponent(out IDamageable damageable) == false || damageable.IsEvasion == false))
            {
                //Move
                transform.position += transform.right * hit.distance;

                //Damage
                if (damageable != null)
                {
                    int damage = CalculatePenetration(_damage, _penetration - _currentPenetration);
                    EEntityPartType parts = damageable.ApplyDamage(damage, hit, transform, transform.right * 4);

                    if (damageable is EntityHealth health && health.TryGetComponent(out Entity entity))
                    {
                        //Effect
                        ParticleSystem wallBloodEffect = gameObject.Pop(EffectPoolType.SpreadWallBlood, hit.point, transform.rotation)
                            .gameObject.GetComponent<ParticleSystem>();
                        var limitVelocityOverLifetimeModule = wallBloodEffect.limitVelocityOverLifetime;

                        //Effect
                        ParticleSystem bloodBackEffect = gameObject.Pop(EffectPoolType.HitBloodBack, hit.point, transform.rotation)
                            .gameObject.GetComponent<ParticleSystem>();

                        if (parts == EEntityPartType.Head)
                        {
                            //Effect
                            gameObject.Pop(EffectPoolType.HitBlood, hit.point, Quaternion.LookRotation(Vector3.back, hit.normal));
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

        public virtual void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner)
        {
            _damage = damage;
            _speed = speed;
            _penetration = penetration;
            _currentPenetration = penetration;
            _whatIsTarget = whatIsTarget;
            transform.right = direction;
            _spriteRenderer.enabled = true;
            _trailRenderer.enabled = true;
            _collider.enabled = true;
            _penetratedColliderList = new List<Collider2D>();
        }

        public override void Die()
        {
            base.Die();
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
        }

        public override void DelayDie()
        {
            base.DelayDie();
            _trailRenderer.enabled = false;
        }
    }
}
