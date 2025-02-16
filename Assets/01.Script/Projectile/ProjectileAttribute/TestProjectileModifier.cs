using UnityEngine;

namespace Hashira.Projectiles
{
    public class TestProjectileModifier : ProjectileModifier
    {
        public override void OnCreated(Projectile projectile)
        {
            base.OnCreated(projectile);
            Debug.Log("���� ��������");
        }

        public override void OnHited(RaycastHit2D hit)
        {
            base.OnHited(hit);
            Debug.Log("���� " + hit.transform.name + "�� ��������");
        }
    }
}
