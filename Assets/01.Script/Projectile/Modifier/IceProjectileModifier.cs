using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class IceProjectileModifier : ProjectileModifier
    {
        [field: SerializeField]
        public float Duration { get; private set; }
        [field: SerializeField]
        public float Amount { get; private set; }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            Debug.Log("작동해라");
        }

        public override void OnProjectileHit(Projectile projectile, HitInfo hitInfo)
        {
            if (projectile == null)
                return;
            base.OnProjectileHit(projectile, hitInfo);
            EntityHealth entityHealth = hitInfo.damageable as EntityHealth;
            if (entityHealth != null)
            {
                var effector = entityHealth.Owner.GetEntityComponent<EntityEffector>();
                var effect = new RubberBullet();
                effect.Duration = Duration;
                effect.Amount = Amount;
                effector.AddEffect(effect);
            }
        }
    }
}
