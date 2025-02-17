using Crogen.CrogenPooling;
using Hashira.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Projectile : PushLifetime
    {
        [SerializeField] protected bool _canMultipleAttacks;
        [SerializeField] protected ProjectileCollider2D _projectileCollider;
        [SerializeField] protected TrailRenderer _trailRenderer;
        protected float _speed;
        public int Damage { get; protected set; }
        protected int _penetration;
        protected int _currentPenetration;
        protected LayerMask _whatIsTarget;
        protected SpriteRenderer _spriteRenderer;
        protected BoxCollider2D _collider;

        protected List<ProjectileModifier> _projectileModifiers;

        protected List<Collider2D> _penetratedColliderList = new List<Collider2D>();
        public Transform Owner { get; set; }

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
            List<RaycastHit2D> newHitList = hits.Where(hit => _penetratedColliderList.Contains(hit.collider) == false).ToList();
            if (_canMultipleAttacks == false && newHitList.Count > 1)
                newHitList = new List<RaycastHit2D>() { newHitList[0] };

            if (isHit && newHitList.Count > 0)
            {
                float movedDistance = 0;
                bool isAnyHit = false;

                //Damage
                newHitList.ForEach(hit =>
                {
                    //Move
                    transform.position += transform.right * (hit.distance - movedDistance);
                    movedDistance += hit.distance;

                    if (hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        if (damageable.IsEvasion == false)
                        {
                            OnHitedDamageable(hit, damageable);

                            isAnyHit = true;
                            _projectileModifiers.ForEach(modifire => modifire.OnHitedDamageable(hit, damageable));
                        }
                    }
                    else
                    {
                        OnHited(hit);

                        isAnyHit = true;
                        _projectileModifiers.ForEach(modifire => modifire.OnHited(hit));
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
                });

                if (isAnyHit == false)
                    transform.position += movement;
            }
            else
                transform.position += movement;
        }

        public int CalculatePenetration(float damage, int penetratedCount)
        {
            if (penetratedCount == 0) return Mathf.CeilToInt(damage);
            return CalculatePenetration(damage * 0.8f, penetratedCount - 1);
        }

        protected virtual void OnHitedDamageable(RaycastHit2D hit, IDamageable damageable)
        {
            OnHited(hit);
        }
        protected virtual void OnHited(RaycastHit2D hit)
        {

        }

        public virtual void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner, List<ProjectileModifier> projectileModifiers = null)
        {
            Damage = damage;
            _speed = speed;
            _penetration = penetration;
            _currentPenetration = penetration;
            _whatIsTarget = whatIsTarget;
            transform.right = direction;
            _spriteRenderer.enabled = true;
            _trailRenderer.enabled = true;
            _collider.enabled = true;
            _penetratedColliderList = new List<Collider2D>();
            Owner = owner;

            _projectileModifiers = projectileModifiers;
            _projectileModifiers ??= new List<ProjectileModifier>();
            _projectileModifiers.ForEach(modifire => modifire.OnCreated(this));
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
