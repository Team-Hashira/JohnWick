using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.Projectiles
{
    //무장탄띠 기능 보류
    public class GunStatIncreaseProjectileModifier : ProjectileModifier
    {
        private StatElement _statElement;

        public void SetIncrease(StatElement statElement, string key, float value)
        {
            _statElement = statElement;
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);
            //_statElement.AddModify("");
        }
    }
}
