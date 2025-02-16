using UnityEngine;

namespace Hashira.Projectiles
{
    public class ProjectileModifier
    {
        public virtual void OnCreated(Projectile projectile)
        {

        }
        public virtual void OnHited(RaycastHit2D hit)
        {

        }
    }
}
