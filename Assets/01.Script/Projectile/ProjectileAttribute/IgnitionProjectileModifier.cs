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

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            if (damageable is EntityHealth entityHealth)
            {
                EffectManager.Instance.AddEffect<Ignition>(entityHealth.Owner, ignitionDamage);
            }
        }
    }
}
