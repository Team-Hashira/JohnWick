using Crogen.CrogenPooling;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class BonusProjectileModifier : ProjectileModifier
    {
        [SerializeField] private ProjectilePoolType _poolType;
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private float _speed;
        [SerializeField] private float _damagePercent;
        [SerializeField] private int _penetration;


        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            Projectile subProjectile = PopCore.Pop(_poolType, projectile.transform.position, Quaternion.identity) as Projectile;
            subProjectile.Init(_whatIsTarget, projectile.transform.right, _speed, Mathf.CeilToInt(projectile.Damage * _damagePercent), _penetration, projectile.Owner);
            subProjectile.transform.rotation = projectile.transform.rotation;
        }
    }
}
