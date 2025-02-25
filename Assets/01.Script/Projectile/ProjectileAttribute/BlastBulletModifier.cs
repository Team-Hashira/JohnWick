using Crogen.CrogenPooling;
using Hashira.Items.Modules;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class BlastBulletModifier : ProjectileModifier
    {
        public int doubleAttackDamage = 100;
        private Projectile _projectiles;

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);
            Debug.Log("dfdfdf");
            for (int i = 0; i < 3; i++)
            {
                var blastBulletModuleSplinter = PopCore.Pop(ProjectilePoolType.BlastBulletModuleSplinter, _projectiles.transform.position, Quaternion.identity) as BlastBulletModuleSplinter;
                Vector2 dir = Random.insideUnitCircle;
                dir.y *= Mathf.Sign(dir.y);
                dir.Normalize();
                blastBulletModuleSplinter.Init(dir, doubleAttackDamage);
            }
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            _projectiles = projectile;
            doubleAttackDamage = (int)(projectile.Damage * 0.3f);
        }
    }
}
