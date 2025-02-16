using Crogen.CrogenPooling;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Grenade : Projectile
    {
        private Vector3 _direction;
        private float _minMaxAngle = 10f;
        private float _angle;
        private float _rotateSpeed = 150f;
        private bool _isUp;

        public override void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner, List<ProjectileModifier> projectileModifiers = default)
        {
            base.Init(whatIsTarget, direction, speed, damage, penetration, owner, projectileModifiers);
            _angle = Random.Range(-_minMaxAngle, _minMaxAngle);
            _isUp = false;
            _direction = direction;
        }

        protected override void FixedUpdate()
        {
            _angle += Time.deltaTime * (_isUp ? _rotateSpeed : -_rotateSpeed);

            if (_angle > _minMaxAngle)
            {
                _angle = _minMaxAngle;
                _isUp = false;
            }
            else if (_angle < -_minMaxAngle)
            {
                _angle = -_minMaxAngle;
                _isUp = true;
            }

            _speed *= (1 + Time.deltaTime * 3);

            transform.right = Quaternion.Euler(0, 0, _angle) * _direction;
            base.FixedUpdate();
        }

        public override void Die()
        {
            gameObject.Pop(EffectPoolType.BoomFire, transform.position, Quaternion.identity);
            base.Die();
        }
    }
}
