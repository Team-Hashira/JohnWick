using Crogen.CrogenPooling;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Items.PartsSystem;
using Hashira.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira.Items.Weapons
{
    public class GunWeapon : Weapon
    {
        public GunSO GunSO { get; private set; }

        public event Action<int> OnFireEvent;
        public int BulletAmount { get; protected set; }

        //Parts
        private readonly Dictionary<EWeaponPartsType, WeaponParts> _partsSlotDictionary = new Dictionary<EWeaponPartsType, WeaponParts>();
        public event Action<EWeaponPartsType, WeaponParts> OnPartsChanged;

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

            //Soundgenerate
            bool isEquipedSoundSuppressor = _partsSlotDictionary[EWeaponPartsType.Muzzle]?.WeaponPartsSO.itemName == "SoundSuppressor";
            if (isEquipedSoundSuppressor == false)
            {
                SoundGeneratedEvent soundGenerated = SoundEvents.SoundGeneratedEvent;
                soundGenerated.originPosition = EntityWeapon.transform.position;
                soundGenerated.loudness = isEquipedSoundSuppressor ? 0 : 10;
                EntityWeapon.GameEventChannelSO.RaiseEvent(soundGenerated);
            }

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

        public override void WeaponUpdate()
        {
            base.WeaponUpdate();
            foreach (WeaponParts parts in _partsSlotDictionary.Values)
            {
                parts?.PartsUpdate();
            }
        }

        public WeaponParts EquipParts(EWeaponPartsType eWeaponPartsType, WeaponParts parts)
        {
            if (_partsSlotDictionary.ContainsKey(eWeaponPartsType) == false) return parts;

            WeaponParts prevPartsSO = _partsSlotDictionary[eWeaponPartsType];

            _partsSlotDictionary[eWeaponPartsType]?.UnEquip();
            _partsSlotDictionary[eWeaponPartsType] = parts;
            _partsSlotDictionary[eWeaponPartsType]?.Equip(this);

            OnPartsChanged?.Invoke(eWeaponPartsType, parts);

            return prevPartsSO;
        }
        public WeaponParts GetParts(EWeaponPartsType eWeaponPartsType)
        {
            if (_partsSlotDictionary.TryGetValue(eWeaponPartsType, out WeaponParts weaponParts))
                return weaponParts;
            else
                return null;
        }
        public bool TryGetParts(EWeaponPartsType eWeaponPartsType, out WeaponParts weaponParts)
        {
            if (_partsSlotDictionary.TryGetValue(eWeaponPartsType, out WeaponParts parts))
            {
                weaponParts = parts;
                return weaponParts != null;
            }
            weaponParts = null;
            return false;
        }

        public void Reload()
        {
            BulletAmount = GunSO.MaxBulletAmount;
        }

        public override object Clone()
        {
            GunSO = WeaponSO as GunSO;
            _partsSlotDictionary.Clear();
            foreach (EWeaponPartsType partsType in GunSO.partsEquipPosDict.Keys)
            {
                _partsSlotDictionary.Add(partsType, null);
            }
            Reload();
            return base.Clone();
        }
    }
}