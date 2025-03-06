using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Projectiles
{
    //Todo
    public class RubberProjectileModifier : ProjectileModifier
    {
        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
        }

        public override void OnProjectileHit(Projectile projectile, HitInfo hitInfo)
        {
            base.OnProjectileHit(projectile, hitInfo);
        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
        }
    }
}
