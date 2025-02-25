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
        protected override void OnHited(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnHited(hit, damageable);
            EntityHealth health = damageable as EntityHealth;
            if(health != null)
            {
                if(health.Owner.TryGetEntityComponent(out EntityEffector effector))
                {
                    DecreaseMoveSpeed effect = new DecreaseMoveSpeed();
                    effect.Duration = 3f;
                    effector.AddEffect(effect);
                }
            }
        }
    }
}
