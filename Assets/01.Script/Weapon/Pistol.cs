using Crogen.CrogenPooling;
using Hashira.Projectile;
using UnityEngine;

namespace Hashira.Weapons
{
    public class Pistol : Gun
    {
        public override bool MainAttack(int damage)
        {
            if (base.MainAttack(damage) == false) return false;

            CameraManager.Instance.ShakeCamera(8, 10, 0.15f);

            //Bullet
            Bullet bullet = gameObject.Pop(_bullet, _firePoint.position, Quaternion.identity) as Bullet;
            bullet.Init(_whatIsTarget, transform.right, _bulletSpeed, Mathf.CeilToInt(damage * _damageCoefficient / 100));
            //Effect
            gameObject.Pop(_fireSpakleEffect, _firePoint.position, Quaternion.LookRotation(Vector3.back, transform.right));

            return true;
        }

        public override void MeleeAttack(int damage)
        {
            base.MeleeAttack(damage);
        }
    }
}
