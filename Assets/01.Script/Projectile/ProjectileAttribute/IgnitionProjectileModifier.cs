using Hashira.EffectSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira
{
    public class IgnitionProjectileModifier : ProjectileModifier
    {
        private int _ignitionDamage;
        private float _damageDelay;
        private float _ignitionDuration;

        public void Setup(int damage, float damageDelay, float duration)
        {
            _ignitionDamage = damage;
            _damageDelay = damageDelay;
            _ignitionDuration = duration;
        }


        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            if (damageable is EntityHealth entityHealth)
            {
                Ignition ignition = new Ignition();
                ignition.Setup(_ignitionDamage, _damageDelay, _ignitionDuration);
                if (entityHealth.Owner.TryGetEntityComponent(out EntityEffector effector, true))
                {
                    effector.AddEffect(ignition);
                }
            }
        }
    }
}
