using Crogen.CrogenPooling;
using Hashira.Projectile;
using UnityEngine;

namespace Hashira.Weapons
{
    public class PistolWeapon : GunWeapon
    {
        private int _damage;

        public override void Attack(int damage, bool isDown)
        {
            if (isDown == false) return;

            if (Fire() == false) return;

            _damage = damage;
            base.Attack(damage, isDown);
            CameraManager.Instance.ShakeCamera(8, 10, 0.15f);
        }

        protected override bool Fire()
        {
            if (base.Fire() == false) return false;

            Vector3 firePos = _EntityWeapon.transform.position + _EntityWeapon.transform.rotation * GunSO._firePoint;
            //Bullet
            Bullet bullet = _EntityWeapon.gameObject.Pop(GunSO._bullet, firePos, Quaternion.identity) as Bullet;
            bullet.Init(GunSO.WhatIsTarget, _EntityWeapon.transform.right, GunSO._bulletSpeed, Mathf.CeilToInt(_damage * GunSO._damageCoefficient / 100));
            //Effect
            _EntityWeapon.gameObject.Pop(GunSO._fireSpakleEffect, firePos, Quaternion.LookRotation(Vector3.back, _EntityWeapon.transform.right));
            return true;
        }
    }
}
