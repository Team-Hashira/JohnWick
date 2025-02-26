using Hashira.Projectiles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira
{
    public class HomingBullet : Bullet
    {
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private float _radius;
        [SerializeField] private float _homingPower;
        private Collider2D _currentTarget;
        //private LayerMask _whatIsTarget;

        public override void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner, List<ProjectileModifier> projectileModifiers = null, AnimationCurve damageOverDistance = null)
        {
            base.Init(whatIsTarget, direction, speed, damage, penetration, owner, projectileModifiers, damageOverDistance);
            //_whatIsTarget = WhatIsTarget & ~(WhatIsTarget & _whatIsObstacle);
        }

        protected override void FixedUpdate()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius, _whatIsTarget);
            if (colliders.Length > 0)
            {
                Collider2D targetCollider = null;
                float minDistance = int.MaxValue;
                foreach (Collider2D collider in colliders)
                {
                    if (_penetratedColliderList.Contains(collider)) continue;
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        targetCollider = collider;
                    }
                }
                _currentTarget = targetCollider;
                transform.right = (transform.right * 100 + (_currentTarget.transform.position - transform.position).normalized * _homingPower).normalized;
            }

            base.FixedUpdate();
        }
    }
}
