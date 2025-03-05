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

        private bool isFirst = false;

        public override void OnProjectileCreateReady()
        {
            base.OnProjectileCreateReady();
            isFirst = true;
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            if (isFirst)
            {
                isFirst = false;
                Projectile subProjectile = _projectile.gameObject.Pop(_poolType, _projectile.transform.position, Quaternion.identity) as Projectile;
                subProjectile.Init(_whatIsTarget, _projectile.transform.right, _speed, Mathf.CeilToInt(_projectile.Damage * _damagePercent), _penetration, _projectile.Owner);
                subProjectile.transform.rotation = _projectile.transform.rotation;
                //ModifierExecuter.Reset();
            }
        }
    }
}
