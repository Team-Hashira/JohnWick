using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Projectile;
using System;
using UnityEngine;

namespace Hashira.Players
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform _visualTrm;
        [SerializeField] private Transform _slideTrm;
        [SerializeField] private ParticleSystem _cartridgeCaseParticle;
        [SerializeField] private LayerMask _whatIsTarget;

        [SerializeField] private Transform _firePoint;
        [SerializeField] private ProjectilePoolType _bullet;
        [SerializeField] private EffectPoolType _fireSpakleEffect;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _damageCoefficient;

        [SerializeField] private BoxDamageCaster2D _boxDamageCaster2D;

        private Sequence _slideBackSeq;
        private Sequence _meleeAttackSeq;

        private float _facing;
        private Vector2 _meleeAttackCenter;

        private void Awake()
        {
            _boxDamageCaster2D.OnDamageCastSuccessEvent += HandleDamageSuccessEvent;
            _meleeAttackCenter = _boxDamageCaster2D.center;
        }

        private void HandleDamageSuccessEvent()
        {
            CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
        }

        public void LookTarget(Vector3 targetPos)
        {
            _facing = MathF.Sign(targetPos.x - transform.position.x);
            transform.right = targetPos - transform.position;
            _visualTrm.localScale = new Vector3(1, _facing, 1);
            Vector2 center = _meleeAttackCenter;
            center.y *= _facing;
            _boxDamageCaster2D.center = center;
        }

        public void Fire(int damage)
        {
            Vector3 movePos = _slideTrm.localPosition;
            movePos.x = -0.2f;
            _slideTrm.localPosition = movePos;
            if (_slideBackSeq != null && _slideBackSeq.IsActive()) _slideBackSeq.Kill();
            _slideBackSeq = DOTween.Sequence();
            _slideBackSeq.Append(_slideTrm.DOLocalMoveX(0f, 0.15f).SetEase(Ease.InSine));


            _cartridgeCaseParticle.Play();

            //Bullet
            Bullet bullet = gameObject.Pop(_bullet, _firePoint.position, Quaternion.identity) as Bullet;
            bullet.Init(_whatIsTarget, transform.right, _bulletSpeed, Mathf.CeilToInt(damage * _damageCoefficient / 100));
            //Effect
            gameObject.Pop(_fireSpakleEffect, _firePoint.position, Quaternion.LookRotation(Vector3.back, transform.right));
        }

        public void MeleeAttack(int damage)
        {
            Vector3 prevPos = _visualTrm.localPosition;
            _visualTrm.localPosition = prevPos + new Vector3(0.1f, 0.2f * _facing, 0f);
            _visualTrm.localEulerAngles = new Vector3(0, 0, 110 * _facing);
            if (_meleeAttackSeq != null && _meleeAttackSeq.IsActive()) _meleeAttackSeq.Kill();
            _meleeAttackSeq = DOTween.Sequence();
            _meleeAttackSeq.Append(_visualTrm.DOLocalMove(prevPos, 0.15f).SetEase(Ease.InSine));
            _meleeAttackSeq.Join(_visualTrm.DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.InSine));

            _boxDamageCaster2D.CastDamage(damage, transform.right * 2.5f);
        }
    }
}