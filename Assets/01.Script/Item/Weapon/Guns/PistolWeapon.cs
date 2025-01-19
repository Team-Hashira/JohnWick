using Crogen.CrogenPooling;
using Hashira.Projectile;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class PistolWeapon : GunWeapon
    {
        public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
            if (isDown == false) return;

            if (Fire() == false) return;

            CameraManager.Instance.ShakeCamera(8, 10, 0.15f);
        }

        protected override bool Fire()
        {
            if (base.Fire() == false) return false;

            Vector3 firePos = _EntityWeapon.transform.position + _EntityWeapon.transform.rotation * GunSO._firePoint;
            CreateBullet(firePos);
            //Effect
            _EntityWeapon.gameObject.Pop(GunSO._fireSpakleEffect, firePos, Quaternion.LookRotation(Vector3.back, _EntityWeapon.transform.right));
            return true;
        }
    }
}
