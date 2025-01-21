using Crogen.CrogenPooling;
using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class RifleGun : GunWeapon
    {
        private StatElement _attackSpeedStat;
        private float _lastFireTime;

        private bool _isFiring;
        private int _damage;
        private float _maxTime;

        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);
            _attackSpeedStat = StatDictionary["AttackSpeed"];
        }

        public override void Equip(EntityWeapon entityWeapon)
        {
            base.Equip(entityWeapon);
            _EntityWeapon.OnCurrentWeaponChanged += HandleCurrentWeaponChangedEvent;
            _EntityWeapon.OnReloadEvent += HandleReloadEvent;
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
            _EntityWeapon.OnCurrentWeaponChanged -= HandleCurrentWeaponChangedEvent;
            _EntityWeapon.OnReloadEvent -= HandleReloadEvent;
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

            Vector3 firePos = _EntityWeapon.VisualTrm.position + _EntityWeapon.transform.rotation * GunSO.firePoint;
            CreateBullet(firePos);
            //Effect
            _EntityWeapon.gameObject.Pop(GunSO.fireSpakleEffect, firePos, Quaternion.LookRotation(Vector3.back, _EntityWeapon.transform.right));

            return true;
        }

        public override void WeaponUpdate()
        {
            if (_isFiring && _lastFireTime + _attackSpeedStat.Value < Time.time)
            {
                if (Fire()) _lastFireTime = Time.time;
                else _isFiring = false;
            }
        }
    }
}
