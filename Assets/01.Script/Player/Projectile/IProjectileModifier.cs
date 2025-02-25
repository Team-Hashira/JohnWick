using System;
using UnityEngine;

namespace Hashira.Projectiles
{
    public interface IProjectileModifier : ICloneable
    {
        //총알이 생겨날 당시
        public void OnProjectileCreate(Projectile projectile);

        //총알의 Update시작 전
        public void OnProjectileUpdate();

        //총알이 충돌시
        public void OnProjectileHit(RaycastHit2D hit, IDamageable damageable);
    }
}
