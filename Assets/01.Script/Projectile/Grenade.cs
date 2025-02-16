using Crogen.CrogenPooling;
using Hashira.Projectile;
using UnityEngine;

namespace Hashira
{
    public class Grenade : Bullet
    {
        public override void Die()
        {
            gameObject.Pop(EffectPoolType.BoomFire, transform.position, Quaternion.identity);
            base.Die();
        }
    }
}
