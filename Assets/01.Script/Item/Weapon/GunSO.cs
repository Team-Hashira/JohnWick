using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace Hashira.Items.Weapons
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon/Gun")]
    public class GunSO : WeaponSO
    {
        [Header("==========Gun setting==========")]
        public Vector3 cartridgeCaseParticlePoint;
        public ProjectilePoolType bullet = ProjectilePoolType.Bullet;
        public EffectPoolType fireSpakleEffect = EffectPoolType.BulletShootSparkleEffect;
        public float bulletSpeed = 200;
        public float reloadDuration = 1;
        [Header("Parts")]
        public SerializedDictionary<EWeaponPartsType, Vector2> partsEquipUIPosDict
            = new SerializedDictionary<EWeaponPartsType, Vector2>();
        public SerializedDictionary<EWeaponPartsType, Vector2Int> partsEquipPosDict
            = new SerializedDictionary<EWeaponPartsType, Vector2Int>();
    }
}
