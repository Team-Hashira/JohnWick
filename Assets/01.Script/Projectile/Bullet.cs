using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Hashira.Projectile
{
    public class Bullet : PushLifetime, IParryingable
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
        private BoxCollider2D _collider;

        private List<Collider2D> _penetratedColliderList = new List<Collider2D>();

        public bool IsParryingable { get; set; }
        public Transform Owner { get; set; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }

        private void FixedUpdate()
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

        public void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner)
        {
            _damage = damage;
            _speed = speed;
            _penetration = penetration;
            _currentPenetration = penetration;
            _whatIsTarget = whatIsTarget;
            transform.right = direction;
            Owner = owner;
            _spriteRenderer.enabled = true;
            _trailRenderer.enabled = true;
            _collider.enabled = true;
            IsParryingable = true;
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

        public void Parrying(LayerMask whatIsNewTargetLayer, Transform owner, bool isChargedParrying)
        {
            if (IsParryingable == false) return;

            Quaternion effectRotation = transform.rotation * Quaternion.Euler(0, 0, -90);
            gameObject.Pop(EffectPoolType.HitSparkleEffect, transform.position, effectRotation);

            if (isChargedParrying)
            {
                CameraManager.Instance.ShakeCamera(15, 11, 0.25f);
                _damage *= 10;
                _speed *= 10;
                gameObject.Pop(EffectPoolType.HitSparkleEffect, transform.position, effectRotation);
            }
            else
                CameraManager.Instance.ShakeCamera(4, 4, 0.15f);

            _whatIsTarget = whatIsNewTargetLayer;
            IsParryingable = false;
            Owner = owner;
            transform.localEulerAngles += new Vector3(0, 180, 0);
        }
    }
}