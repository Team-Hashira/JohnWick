using System;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class ProjectileModifier : ICloneable
    {
        protected Projectile _projectile;
        protected ProjectileModifierSO _projectileModifierSO;
        protected Attacker _attacker;

        /// <summary>
        /// 가장 처음 클래스 생성시
        /// </summary>
        /// <param name="projectileModifierSO"></param>
        public void Init(ProjectileModifierSO projectileModifierSO)
        {
            _projectileModifierSO = projectileModifierSO;
        }

        /// <summary>
        /// 수정자가 추가될 때
        /// </summary>
        /// <param name="projectile"></param>
        public virtual void OnEquip(Attacker attacker)
        {
            _attacker = attacker;
        }

        /// <summary>
        /// 수정자가 빠질 때
        /// </summary>
        /// <param name="projectile">수param>
        public virtual void OnUnEquip()
        {

        }

        /// <summary>
        /// 총알이 생겨날 당시
        /// </summary>
        /// <param name="projectile"></param>
        public virtual void OnProjectileCreate(Projectile projectile)
        {
            _projectile = projectile;
        }

        /// <summary>
        /// Update시작 전
        /// </summary>
        public virtual void OnUpdate()
        {

        }

        /// <summary>
        /// 총알의 Update시작 전
        /// </summary>
        public virtual void OnProjectileUpdate()
        {

        }

        /// <summary>
        /// 총알이 충돌시
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="damageable"></param>
        public virtual void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {

        }

        /// <summary>
        /// 클론 할때마다
        /// </summary>
        /// <returns></returns>
        public object Clone() => MemberwiseClone();
    }
}
