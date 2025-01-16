using Crogen.CrogenPooling;
using Hashira.Projectile;
using UnityEngine;

namespace Hashira.Items
{
    public class Rifle : Gun
    {
        [SerializeField] private float _autoSpeed = 0.15f;
        [SerializeField] private float _lastFireTime;
        
        private bool _isFiring;
        private int _damage;

        private void OnEnable()
        {
            _isFiring = false;
        }

        public override void MainAttack(int damage, bool isDown)
        {
            if (BulletAmount <= 0) return;

            base.MainAttack(damage, isDown);
            _isFiring = isDown;
            _damage = damage;
        }

        protected override bool Fire()
        {
            if (base.Fire() == false) return false;
            _lastFireTime = Time.time;

            CameraManager.Instance.ShakeCamera(8, 10, 0.15f);

            //Bullet
            Bullet bullet = gameObject.Pop(_bullet, _firePoint.position, Quaternion.identity) as Bullet;
            bullet.Init(_whatIsTarget, transform.right, _bulletSpeed, Mathf.CeilToInt(_damage * _damageCoefficient / 100));
            //Effect
            gameObject.Pop(_fireSpakleEffect, _firePoint.position, Quaternion.LookRotation(Vector3.back, transform.right));

            return true;
        }

        private void Update()
        {
            if (_isFiring && _lastFireTime + _autoSpeed < Time.time)
            {
                if (Fire()) _lastFireTime = Time.time;
                else _isFiring = false;
            }
        }
    }
}
