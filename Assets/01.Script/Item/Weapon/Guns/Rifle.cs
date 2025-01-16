using Crogen.CrogenPooling;
using Hashira.Projectile;
using UnityEngine;

namespace Hashira.Weapons
{
    public class Rifle : Gun
    {
        private float _autoSpeed = 0.15f;
        private float _lastFireTime;
        
        private bool _isFiring;
        private int _damage;

        public override void UnEquip()
        {
            base.UnEquip();
            _isFiring = false;
        }

        public override void Attack(int damage, bool isDown)
        {
            if (BulletAmount <= 0) return;

            base.Attack(damage, isDown);
            _isFiring = isDown;
            _damage = damage;
        }

        protected override bool Fire()
        {
            if (base.Fire() == false) return false;
            _lastFireTime = Time.time;

            CameraManager.Instance.ShakeCamera(8, 10, 0.15f);

            Vector3 firePos = _EntityWeapon.transform.position + _EntityWeapon.transform.rotation * GunSO._firePoint;
            //Bullet
            Bullet bullet = _EntityWeapon.gameObject.Pop(GunSO._bullet, firePos, Quaternion.identity) as Bullet;
            bullet.Init(GunSO.WhatIsTarget, _EntityWeapon.transform.right, GunSO._bulletSpeed, Mathf.CeilToInt(_damage * GunSO._damageCoefficient / 100));
            //Effect
            _EntityWeapon.gameObject.Pop(GunSO._fireSpakleEffect, firePos, Quaternion.LookRotation(Vector3.back, _EntityWeapon.transform.right));

            return true;
        }

        public override void WeaponUpdate()
        {
            if (_isFiring && _lastFireTime + _autoSpeed < Time.time)
            {
                if (Fire()) _lastFireTime = Time.time;
                else _isFiring = false;
            }
        }
    }
}
