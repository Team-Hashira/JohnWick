using Hashira.EffectSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira
{
    public class IgnitionProjectileModifier : ProjectileModifier
    {
        public int ignitionLevel;
        public float ignitionDuration;

        public override void OnCreated(Projectile projectile)
        {
            base.OnCreated(projectile);
        }

        public override void OnHitedDamageable(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnHitedDamageable(hit, damageable);
            if (damageable is EntityHealth entityHealth)
            {
                EffectManager.Instance.AddEffect<Ignition>(entityHealth.Owner, ignitionLevel, ignitionDuration);
            }
        }
    }
}
