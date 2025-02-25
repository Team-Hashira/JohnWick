using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class BlastProjectileModifier : ProjectileModifier
    {
        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);

            int damage = (int)(_projectile.Damage * 0.3f);

            for (int i = -1; i <= 1; i++)
            {
                var blastBulletModuleSplinter = PopCore.Pop(ProjectilePoolType.BlastBulletModuleSplinter, _projectile.transform.position + Vector3.up * 0.25f, Quaternion.Euler(0, 0, Random.Range(0, 360f))) as BlastBulletModuleSplinter;
                Vector2 dir = new Vector2(i * 0.55f, 1).normalized * 300;
                dir += Random.insideUnitCircle.normalized * 10f;
                blastBulletModuleSplinter.Init(dir, damage);
            }

            ModifierExecuter.Reset();
        }
    }
}
