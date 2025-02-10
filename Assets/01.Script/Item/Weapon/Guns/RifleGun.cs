using Crogen.CrogenPooling;
using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class RifleGun : GunWeapon
    {
        private float _lastFireTime;

        private bool _isFiring;
        private int _damage;
        private float _maxTime;


        public override void Equip(EntityWeapon entityWeapon)
        {
            base.Equip(entityWeapon);
            EntityGunWeapon.OnCurrentWeaponChanged += HandleCurrentWeaponChangedEvent;
            EntityGunWeapon.OnReloadEvent += HandleReloadEvent;
        }

        private void HandleReloadEvent(float time)
        {
            if (time == 0)
            {
                _maxTime = 0;
            }
            else if (_maxTime < time)
            {
                _maxTime = time;
                _isFiring = false;
            }
        }

        private void HandleCurrentWeaponChangedEvent(Weapon weapon)
        {
            _isFiring = false;
        }

        public override void UnEquip()
        {
            EntityGunWeapon.OnCurrentWeaponChanged -= HandleCurrentWeaponChangedEvent;
            EntityGunWeapon.OnReloadEvent -= HandleReloadEvent;
            base.UnEquip();
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

            Vector3 direction = CalculateRecoil(EntityGunWeapon.transform.right);
            CreateBullet(direction);
            //Effect
            EntityGunWeapon.gameObject.Pop(GunSO.fireSpakleEffect, _firePos, Quaternion.LookRotation(Vector3.back, EntityGunWeapon.transform.right));

            return true;
        }

        public override void WeaponUpdate()
        {
            base.WeaponUpdate();
            if (_isFiring) Fire();
            else if (BulletAmount == 0) _isFiring = false;
        }
    }
}
