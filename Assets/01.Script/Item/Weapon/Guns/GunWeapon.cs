using Crogen.CrogenPooling;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using Hashira.Items.PartsSystem;
using Hashira.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira.Items.Weapons
{
    public class GunWeapon : Weapon
    {
        public GunSO GunSO { get; private set; }
        public EntityWeaponHolder EntityGunWeapon { get; private set; }
        public EntitySoundGenerator EntitySoundGenerator { get; private set; }

        public event Action<int> OnFireEvent;
        public event Action<PartsRenderer> OnPartsRendererChangedEvent;
        public int BulletAmount { get; protected set; }

        //Parts
        private Dictionary<EWeaponPartsType, WeaponParts> _partsSlotDictionary;
        public event Action<EWeaponPartsType, WeaponParts> OnPartsChanged;

        private StatElement _precisionStat;
        private StatElement _recoilStat;
        private StatElement _attackSpeedStat;
        private StatElement _magazineCapacityStat;
        private StatElement _penetrationStat;

        public PartsRenderer PartsRenderer { get; private set; }

        public Vector3 FirePos { get; private set; }
        private float _lastFireTime;

        private List<ProjectileModifier> _projectileModifiers = new List<ProjectileModifier>();

        public bool IsCanFire => _lastFireTime + 1 / _attackSpeedStat.Value < Time.time && EntityGunWeapon.IsStuck == false;

        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);
            GunSO = WeaponSO as GunSO;
        }

        public void SetPartsRenderer(PartsRenderer partsRenderer)
        {
            PartsRenderer = partsRenderer;
            OnPartsRendererChangedEvent?.Invoke(partsRenderer);
        }

        private void HandleDamageSuccessEvent()
        {
            CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
        }

        protected void SpawnCartridgeCase()
        {
            EntityGunWeapon.CartridgeCaseParticle.Play();
        }

        public override void Equip(EntityItemHolder entityWeapon)
        {
            base.Equip(entityWeapon);
            EntityGunWeapon = entityWeapon as EntityWeaponHolder;
			EntitySoundGenerator = entityWeapon.Entity.GetEntityComponent<EntitySoundGenerator>();

		}

        public override void UnEquip()
        {
            EntityGunWeapon = null;
            base.UnEquip();
        }

        protected virtual bool Fire()
        {
            if (BulletAmount <= 0)
            {
                EntityGunWeapon.Reload();
                return false;
            }

            if (IsCanFire)
                _lastFireTime = Time.time;
            else return false;

            CalculateFirePos();

            BulletAmount--;

            //Soundgenerate
            bool isEquipedSoundSuppressor = _partsSlotDictionary.TryGetValue(EWeaponPartsType.Muzzle, out WeaponParts weaponParts) && weaponParts != null && weaponParts.WeaponPartsSO.itemName == "SoundSuppressor";
            if (isEquipedSoundSuppressor == false)
            {
                SoundGeneratedEvent soundGenerated = SoundEvents.SoundGeneratedEvent;
                soundGenerated.originPosition = EntityGunWeapon.transform.position;
                soundGenerated.loudness = isEquipedSoundSuppressor ? 0 : 10;
				EntitySoundGenerator.SoundEventChannel.RaiseEvent(soundGenerated);
            }

            OnFireEvent?.Invoke(BulletAmount);

            SpawnCartridgeCase();

            return true;
        }

        private void CalculateFirePos()
        {
            SpriteRenderer spriteRenderer = EntityGunWeapon.PartsRenderer[EWeaponPartsType.Muzzle];
            Vector3 muzzlePos = spriteRenderer.transform.position;
            float muzzlePartsSize = spriteRenderer.sprite != null ?
                spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit : 0;

            FirePos = muzzlePos + spriteRenderer.transform.right * muzzlePartsSize;
        }

        protected void CreateBullet(Vector3 direction)
        {
            //Bullet
            Bullet bullet = EntityGunWeapon.gameObject.Pop(GunSO.bullet, FirePos, Quaternion.identity) as Bullet;
            bullet.Init(WhatIsTarget, direction, GunSO.bulletSpeed, CalculateDamage(), _penetrationStat.IntValue, EntityGunWeapon.Entity.transform, _projectileModifiers.Select(mmodifier => (ProjectileModifier)mmodifier.Clone()).ToList(), GunSO.damageOverDistance);

            EntityGunWeapon.ApplyRecoil(_recoilStat.Value);
        }

        protected Vector3 CalculateRecoil(Vector3 direction)
        {
            float randomRecoil = Random.Range(-EntityGunWeapon.Recoil, EntityGunWeapon.Recoil);
            Vector3 targetDir = (Quaternion.Euler(0, 0, randomRecoil) * direction).normalized;
            return targetDir;
        }

        public override int CalculateDamage()
        {
            return Mathf.CeilToInt(base.CalculateDamage() * (Random.Range(_precisionStat.Value, 100f - (100f - _precisionStat.Value) / 1.5f)) / 100);
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();
            foreach (WeaponParts parts in _partsSlotDictionary.Values)
            {
                parts?.PartsUpdate();
            }
        }

        public bool CheckWeaponPartsType(EWeaponPartsType eWeaponPartsType)
        {
            return _partsSlotDictionary.ContainsKey(eWeaponPartsType);
        }

        public bool CheckPartsSlotEmpty(EWeaponPartsType eWeaponPartsType)
        {
            return _partsSlotDictionary[eWeaponPartsType] == null;
        }

        public WeaponParts EquipParts(EWeaponPartsType eWeaponPartsType, WeaponParts parts)
        {
            if (CheckWeaponPartsType(eWeaponPartsType) == false) return parts;

            WeaponParts prevPartsSO = _partsSlotDictionary[eWeaponPartsType];

            _partsSlotDictionary[eWeaponPartsType]?.UnEquip();
            _partsSlotDictionary[eWeaponPartsType] = parts;
            _partsSlotDictionary[eWeaponPartsType]?.Equip(this);

            OnPartsChanged?.Invoke(eWeaponPartsType, parts);

            return prevPartsSO;
        }
        public WeaponParts UnEquipParts(EWeaponPartsType eWeaponPartsType)
        {
            if (_partsSlotDictionary.ContainsKey(eWeaponPartsType) == false) return null;

            WeaponParts prevPartsSO = _partsSlotDictionary[eWeaponPartsType];

            _partsSlotDictionary[eWeaponPartsType]?.UnEquip();
            _partsSlotDictionary[eWeaponPartsType] = null;

            OnPartsChanged?.Invoke(eWeaponPartsType, null);

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

        public void AddProjectileModifier(ProjectileModifier projectileModifier)
        {
            _projectileModifiers.Add(projectileModifier);
        }
        public void RemoveProjectileModifier(ProjectileModifier projectileModifier)
        {
            _projectileModifiers.Remove(projectileModifier);
        }

        public void Reload()
        {
            BulletAmount = _magazineCapacityStat.IntValue;
        }

        private void StatElementsSetting()
        {
            _precisionStat = StatDictionary["Precision"];
            _recoilStat = StatDictionary["Recoil"];
            _attackSpeedStat = StatDictionary["AttackSpeed"];
            _magazineCapacityStat = StatDictionary["MagazineCapacity"];
            _penetrationStat = StatDictionary["Penetration"];
        }

        public override object Clone()
        {
            GunWeapon clonedGunWeapon = (GunWeapon)base.Clone();
            clonedGunWeapon._partsSlotDictionary = new Dictionary<EWeaponPartsType, WeaponParts>();
            clonedGunWeapon._partsSlotDictionary.Clear();
            foreach (EWeaponPartsType partsType in GunSO.partsEquipUIPosDict.Keys)
            {
                clonedGunWeapon._partsSlotDictionary.Add(partsType, null);
            }

            clonedGunWeapon.StatElementsSetting();
            clonedGunWeapon.Reload();

            foreach (EWeaponPartsType weaponPartsType in clonedGunWeapon.GunSO.defaultParts.Keys)
            {
                PartsSO partsSO = clonedGunWeapon.GunSO.defaultParts[weaponPartsType];
                WeaponParts weaponParts = (WeaponParts)partsSO.GetItemClass();

                clonedGunWeapon.EquipParts(weaponPartsType, weaponParts);
            }

            return clonedGunWeapon;
        }
    }
}