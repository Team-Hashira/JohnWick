using DG.Tweening;
using Hashira.Projectile;
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
        [SerializeField] private Bullet _bullet;
        [SerializeField] private Transform _fireSpakleEffect;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _damageCoefficient;

        [SerializeField] private DamageCaster2D _damageCaster2D;

        private Sequence _slideBackSeq;
        private Sequence _meleeAttackSeq;

        public void LookTarget(Vector3 targetPos)
        {
            transform.localRotation = Quaternion.Euler(0, 0, -90) *
                Quaternion.LookRotation(Vector3.back, targetPos - transform.position);
            _visualTrm.localRotation = Quaternion.Euler(targetPos.x - transform.position.x < 0 ? 0 : 180, 0, 0);
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
            Bullet bullet = Instantiate(_bullet, _firePoint.position, Quaternion.identity);
            bullet.Init(_whatIsTarget, transform.right, _bulletSpeed, Mathf.CeilToInt(damage * _damageCoefficient / 100));
            //Effect
            Transform fireSpakle = Instantiate(_fireSpakleEffect, _firePoint.position, Quaternion.identity);
            fireSpakle.up = transform.right;
        }

        public void MeleeAttack(int damage)
        {
            Vector3 movePos = _visualTrm.localPosition;
            movePos.x = 0.5f;
            _visualTrm.localPosition = movePos;
            if (_meleeAttackSeq != null && _meleeAttackSeq.IsActive()) _meleeAttackSeq.Kill();
            _meleeAttackSeq = DOTween.Sequence();
            _meleeAttackSeq.Append(_visualTrm.DOLocalMoveX(0f, 0.15f).SetEase(Ease.InSine));

            _damageCaster2D.CastDamage(damage);
        }
    }
}