using Crogen.CrogenPooling;
using DG.Tweening;
using System;
using UnityEngine;

namespace Hashira.Items
{
    public class Gun : Weapon
    {
        [SerializeField] protected Transform _slideTrm;
        [SerializeField] protected ParticleSystem _cartridgeCaseParticle;
        [SerializeField] protected LayerMask _whatIsTarget;

        [SerializeField] protected Transform _firePoint;
        [SerializeField] protected ProjectilePoolType _bullet;
        [SerializeField] protected EffectPoolType _fireSpakleEffect;
        [SerializeField] protected float _bulletSpeed;
        [SerializeField] protected float _damageCoefficient;

        [SerializeField] private BoxDamageCaster2D _boxDamageCaster2D;

        private Vector2 _meleeAttackCenter;

        public event Action<int> OnFireEvent;
        public int BulletAmount { get; protected set; }
        [field: SerializeField] public int MaxBulletAmount { get; protected set; }

        [SerializeField] private float _reloadDuration;

        private Sequence _slideBackSeq;
        private Sequence _meleeAttackSeq;

        protected virtual void Awake()
        {
            BulletAmount = MaxBulletAmount;
            _boxDamageCaster2D.OnDamageCastSuccessEvent += HandleDamageSuccessEvent;
            _meleeAttackCenter = _boxDamageCaster2D.center;
        }

        private void HandleDamageSuccessEvent()
        {
            CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
        }

        public override void MainAttack(int damage, bool isDown)
        {
            Vector3 movePos = _slideTrm.localPosition;
            movePos.x = -0.2f;
            _slideTrm.localPosition = movePos;
            if (_slideBackSeq != null && _slideBackSeq.IsActive()) _slideBackSeq.Kill();
            _slideBackSeq = DOTween.Sequence();
            _slideBackSeq.Append(_slideTrm.DOLocalMoveX(0f, 0.15f).SetEase(Ease.InSine));
        }

        public override void MeleeAttack(int damage)
        {
            Vector3 prevPos = _visualTrm.localPosition;
            _visualTrm.localPosition = prevPos + new Vector3(0.1f, 0.2f, 0f);
            _visualTrm.localEulerAngles = new Vector3(0, 0, 110);
            if (_meleeAttackSeq != null && _meleeAttackSeq.IsActive()) _meleeAttackSeq.Kill();
            _meleeAttackSeq = DOTween.Sequence();
            _meleeAttackSeq.Append(_visualTrm.DOLocalMove(prevPos, 0.15f).SetEase(Ease.InSine));
            _meleeAttackSeq.Join(_visualTrm.DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.InSine));

            _boxDamageCaster2D.CastDamage(damage, transform.right * 2.5f);
        }

        protected void SpawnCartridgeCase()
        {
            _cartridgeCaseParticle.Play();
        }

        protected virtual bool Fire()
        {
            if (BulletAmount <= 0) return false;
            BulletAmount--;
            OnFireEvent?.Invoke(BulletAmount);

            SpawnCartridgeCase();

            return true;
        }

        public void Reload()
        {
            BulletAmount = MaxBulletAmount;
        }
    }
}