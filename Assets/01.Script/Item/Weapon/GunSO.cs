using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Hashira.Items.PartsSystem;

namespace Hashira.Items.Weapons
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Item/Weapon/Gun")]
    public class GunSO : WeaponSO
    {
        [Header("==========Gun setting==========")]
        public Vector3 cartridgeCaseParticlePoint;
        public ProjectilePoolType bullet = ProjectilePoolType.Bullet;
        public EffectPoolType fireSpakleEffect = EffectPoolType.BulletShootSparkleEffect;
        public float bulletSpeed = 200;
        public float reloadDuration = 1;
        [Header("DistanceDamage")]
        public AnimationCurve damageOverDistance;
        [Header("Parts")]
        public SerializedDictionary<EWeaponPartsType, PartsSO> defaultParts
            = new SerializedDictionary<EWeaponPartsType, PartsSO>();
        public SerializedDictionary<EWeaponPartsType, Vector2> partsEquipUIPosDict
            = new SerializedDictionary<EWeaponPartsType, Vector2>();
        public SerializedDictionary<EWeaponPartsType, Vector2Int> partsEquipPosDict
            = new SerializedDictionary<EWeaponPartsType, Vector2Int>();
    }
}
