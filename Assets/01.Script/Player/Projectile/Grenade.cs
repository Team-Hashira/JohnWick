using Crogen.CrogenPooling;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Grenade : Projectile
    {
        private Vector3 _direction;
        //private float _minMaxAngle = 10f;
        //rivate float _angle;
        //private float _rotateSpeed = 150f;
        //private bool _isUp;

        [SerializeField] private CircleDamageCaster2D _circleDamageCaster;

        public override void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner, List<IProjectileModifier> projectileModifiers = default, AnimationCurve damageOverDistance = null)
        {
            base.Init(whatIsTarget, direction, speed, damage, penetration, owner, projectileModifiers, damageOverDistance);
            //_angle = Random.Range(-_minMaxAngle, _minMaxAngle);
            //_isUp = false;
            _direction = direction;
        }

        protected override void OnHited(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnHited(hit, damageable);
            CameraManager.Instance.ShakeCamera(8, 8, 0.2f, DG.Tweening.Ease.InCirc);
            _circleDamageCaster.SetLayerMask(_whatIsTarget);
            _circleDamageCaster.CastDamage(Damage, Vector2.zero, transform.right, attackType: Entities.EAttackType.Fire);
        }

        protected override void FixedUpdate()
        {
            //_angle += Time.deltaTime * (_isUp ? _rotateSpeed : -_rotateSpeed);

            //if (_angle > _minMaxAngle)
            //{
            //    _angle = _minMaxAngle;
            //    _isUp = false;
            //}
            //else if (_angle < -_minMaxAngle)
            //{
            //    _angle = -_minMaxAngle;
            //    _isUp = true;
            //}

            //_speed *= (1 + Time.deltaTime * 3);

            transform.right = /*Quaternion.Euler(0, 0, _angle) * */_direction;
            base.FixedUpdate();
        }

        public override void Die()
        {
            gameObject.Pop(EffectPoolType.BoomFire, transform.position, Quaternion.identity);
            base.Die();
        }
    }
}
