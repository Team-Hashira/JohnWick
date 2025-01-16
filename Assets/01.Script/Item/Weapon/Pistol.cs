using Crogen.CrogenPooling;
using Hashira.Projectile;
using UnityEngine;

namespace Hashira.Items
{
    public class Pistol : Gun
    {
        private int _damage;

        public override void MainAttack(int damage, bool isDown)
        {
            if (isDown == false) return;

            if (Fire() == false) return;

            _damage = damage;
            base.MainAttack(damage, isDown);
            CameraManager.Instance.ShakeCamera(8, 10, 0.15f);
        }

        protected override bool Fire()
        {
            if (base.Fire() == false) return false;

            //Bullet
            Bullet bullet = gameObject.Pop(_bullet, _firePoint.position, Quaternion.identity) as Bullet;
            bullet.Init(_whatIsTarget, transform.right, _bulletSpeed, Mathf.CeilToInt(_damage * _damageCoefficient / 100));
            //Effect
            gameObject.Pop(_fireSpakleEffect, _firePoint.position, Quaternion.LookRotation(Vector3.back, transform.right));
            return true;
        }
    }
}
