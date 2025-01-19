using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon/Gun")]
    public class GunSO : WeaponSO
    {
        [Header("==========Gun setting==========")]
        public Vector2 _firePoint;
        public ProjectilePoolType _bullet = ProjectilePoolType.Bullet;
        public EffectPoolType _fireSpakleEffect = EffectPoolType.BulletShootSpakleEffect;
        public float _bulletSpeed = 200;
        public float _reloadDuration = 1;
        public int MaxBulletAmount { get; protected set; } = 10;
    }
}
