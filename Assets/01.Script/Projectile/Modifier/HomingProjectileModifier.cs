using Hashira.Players;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class HomingProjectileModifier : ProjectileModifier
    {
        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);
        }

        public override void OnProjectileUpdate()
        {
            base.OnProjectileUpdate();
        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}
