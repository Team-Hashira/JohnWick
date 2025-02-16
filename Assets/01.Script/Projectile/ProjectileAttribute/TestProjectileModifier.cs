using UnityEngine;

namespace Hashira.Projectiles
{
    public class TestProjectileModifier : ProjectileModifier
    {
        public override void OnCreated(Projectile projectile)
        {
            base.OnCreated(projectile);
            Debug.Log("È÷È÷ »ý°åÁö·Õ");
        }

        public override void OnHited(RaycastHit2D hit)
        {
            base.OnHited(hit);
            Debug.Log("È÷È÷ " + hit.transform.name + "À» ¶§·ÈÁö·Õ");
        }
    }
}
