using UnityEngine;

namespace Hashira.Projectiles
{
    public class ProjectileModifier
    {
        protected Projectile _projectile;
        public virtual void OnCreated(Projectile projectile)
        {
            _projectile = projectile;
        }
        public virtual void OnHited(RaycastHit2D hit)
        {

        }
    }
}
