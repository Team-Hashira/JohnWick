using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon/Gun")]
    public class GunSO : WeaponSO
    {
        [Header("==========Gun setting==========")]
        public Vector3 firePoint;
        public Vector3 cartridgeCaseParticlePoint;
        public ProjectilePoolType bullet = ProjectilePoolType.Bullet;
        public EffectPoolType fireSpakleEffect = EffectPoolType.BulletShootSparkleEffect;
        public float bulletSpeed = 200;
        public float reloadDuration = 1;
        public int MaxBulletAmount { get; protected set; } = 10;
    }
}
