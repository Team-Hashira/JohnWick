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

        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);
            _precisionStat = StatDictionary["Precision"];
        }

        private void HandleDamageSuccessEvent()
        {
            CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
        }

        protected void SpawnCartridgeCase()
        {
            _EntityWeapon.CartridgeCaseParticle.Play();
        }

        protected virtual bool Fire()
        {
            if (BulletAmount <= 0) return false;
            BulletAmount--;
            OnFireEvent?.Invoke(BulletAmount);

            SpawnCartridgeCase();

            return true;
        }

        protected void CreateBullet(Vector3 firePos)
        {
            //Bullet
            Bullet bullet = _EntityWeapon.gameObject.Pop(GunSO.bullet, firePos, Quaternion.identity) as Bullet;
            bullet.Init(GunSO.WhatIsTarget, _EntityWeapon.transform.right, GunSO.bulletSpeed, CalculateDamage());
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