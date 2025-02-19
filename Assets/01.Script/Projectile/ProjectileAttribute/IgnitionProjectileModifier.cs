using Hashira.EffectSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira
{
    public class IgnitionProjectileModifier : ProjectileModifier
    {
        public int ignitionDamage;
        public float ignitionDuration;

        private Ignition _ignition = new Ignition();

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            _ignition = new Ignition();
            _ignition.damage = 10;
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            if (damageable is EntityHealth entityHealth)
            {
                //EffectManager.Instance.AddEffect(entityHealth.Owner, _ignition);
            }
        }
    }
}
