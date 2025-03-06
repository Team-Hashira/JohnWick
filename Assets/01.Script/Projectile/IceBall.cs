using Hashira.Core.StatSystem;
using Hashira.EffectSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using Hashira.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class IceBall : Projectile
    {
        protected override void OnHited(HitInfo hitInfo)
        {
            base.OnHited(hitInfo);
            EntityHealth health = hitInfo.damageable as EntityHealth;
            if(health != null)
            {
                if(health.Owner.TryGetEntityComponent(out EntityEffector effector))
                {
                    RubberBullet effect = new RubberBullet();
                    effect.Duration = 3f;
                    effector.AddEffect(effect);
                }
            }
        }
    }
}
