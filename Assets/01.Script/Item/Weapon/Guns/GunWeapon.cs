using Crogen.CrogenPooling;
using Hashira.Core.StatSystem;
using Hashira.Projectile;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira.Items.Weapons
{
    public class GunWeapon : Weapon
    {
        public GunSO GunSO { get; private set; }

        public event Action<int> OnFireEvent;
        public int BulletAmount { get; protected set; }

        private StatElement _precisionStat;
        private StatElement _recoilStat;
        private StatElement _attackSpeedStat;

        private float _lastFireTime;

        public bool IsCanFire => _lastFireTime + 1 / _attackSpeedStat.Value < Time.time;

        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);
            _precisionStat = StatDictionary["Precision"];
            _recoilStat = StatDictionary["Recoil"];
            _attackSpeedStat = StatDictionary["AttackSpeed"];
        }

        private void HandleDamageSuccessEvent()
        {
            CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
        }

        protected void SpawnCartridgeCase()
        {
            EntityWeapon.CartridgeCaseParticle.Play();
        }

        protected virtual bool Fire()
        {
            if (BulletAmount <= 0) return false;

            if (IsCanFire)
                _lastFireTime = Time.time;
            else return false;

            BulletAmount--;

            OnFireEvent?.Invoke(BulletAmount);

            SpawnCartridgeCase();

            return true;
        }

        protected void CreateBullet(Vector3 firePos, Vector3 direction)
        {
            //Bullet
            Bullet bullet = EntityWeapon.gameObject.Pop(GunSO.bullet, firePos, Quaternion.identity) as Bullet;
            bullet.Init(GunSO.WhatIsTarget, direction, GunSO.bulletSpeed, CalculateDamage());

            EntityWeapon.ApplyRecoil(_recoilStat.Value);
        }

        protected Vector3 CalculateRecoil(Vector3 direction)
        {
            float randomRecoil = Random.Range(-EntityWeapon.Recoil, EntityWeapon.Recoil);
            Vector3 targetDir = (Quaternion.Euler(0, 0, randomRecoil) * direction).normalized;
            return targetDir;
        }

        public override int CalculateDamage()
        {
            return Mathf.CeilToInt(base.CalculateDamage() * (Random.Range(_precisionStat.Value, 100f - (100f - _precisionStat.Value) / 1.5f)) / 100);
        }

        public void Reload()
        {
            BulletAmount = GunSO.MaxBulletAmount;
        }

        public override object Clone()
        {
            GunSO = WeaponSO as GunSO;
            Reload();
            return base.Clone();
        }
    }
}