using UnityEngine;

namespace Hashira.Projectiles
{
    public class ProjectileModifier
    {
        protected Projectile _projectile;
        //총알이 생겨날 당시
        public virtual void OnProjectileCreate(Projectile projectile)
        {
            _projectile = projectile;
        }
        //총알이 충돌시
        public virtual void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {

        }
    }
}
