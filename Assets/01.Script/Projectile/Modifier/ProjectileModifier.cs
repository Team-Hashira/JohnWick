using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class ProjectileModifier
    {
        public ProjectileModifierSO ProjectileModifierSO { get; private set; }
        protected Attacker _attacker;
        //public ModifierExecuter ModifierExecuter { get; private set; }

        /// <summary>
        /// 가장 처음 클래스 생성시
        /// </summary>
        /// <param name="projectileModifierSO"></param>
        public void Init(ProjectileModifierSO projectileModifierSO)
        {
            ProjectileModifierSO = projectileModifierSO;
        }

        /// <summary>
        /// 수정자가 추가될 때
        /// </summary>
        public virtual void OnEquip(Attacker attacker)
        {
            _attacker = attacker;
        }

        /// <summary>
        /// 수정자가 빠질 때
        /// </summary>
        public virtual void OnUnEquip()
        {

        }

        /// <summary>
        /// 총알이 생겨난 직후
        /// </summary>
        /// <param name="projectile"></param>
        public virtual void OnProjectileCreate(Projectile projectile)
        {

        }

        /// <summary>
        /// Update시작 전
        /// </summary>
        public virtual void OnUpdate(Projectile projectile)
        {

        }

        /// <summary>
        /// 총알이 충돌시
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="damageable"></param>
        public virtual void OnProjectileHit(Projectile projectile, HitInfo hitInfo)
        {

        }
    }
}
