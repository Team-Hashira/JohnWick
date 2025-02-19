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

        private Ignition _ignition = new Ignition();

        public void Setup(int damage, float damageDelay, float duration)
        {
            _ignitionDamage = damage;
            _damageDelay = damageDelay;
            _ignitionDuration = duration;
        }


        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            _ignition = new Ignition();
            _ignition.Setup(_ignitionDamage, _damageDelay, _ignitionDuration);
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            if (damageable is EntityHealth entityHealth)
            {
                EffectManager.Instance.AddEffect(entityHealth.Owner, _ignition);
            }
        }
    }
}
