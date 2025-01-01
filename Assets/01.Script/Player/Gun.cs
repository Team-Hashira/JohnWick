using DG.Tweening;
using UnityEngine;

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

    private Sequence _slideBackSeq;

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
}
