using Crogen.CrogenPooling;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Hashira.Projectiles
{
    //마킹인데 나중에 쓸까봐 냅둔다
    public class MarkAttackProjectileModifier : ProjectileModifier
    {
        private static Dictionary<EntityHealth, IPoolingObject> _markedEntityHealthList = new Dictionary<EntityHealth, IPoolingObject>();
        public int damage = 500;

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
        }

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);
            if (damageable != null && damageable is EntityHealth entityHealth && 
                entityHealth.Owner != GameManager.Instance.Player && _markedEntityHealthList.ContainsKey(entityHealth) == false)
            {
                entityHealth.OnDieEvent += HandleDieEvent;
                IPoolingObject mark = _projectile.gameObject.Pop(OtherPoolType.LaserMark, entityHealth.Owner.transform);
                _markedEntityHealthList.Add(entityHealth, mark);

                HandleDieEvent();
                if (_markedEntityHealthList.Count >= 3)
                {
                    MarkAttack();
                    _markedEntityHealthList = new Dictionary<EntityHealth, IPoolingObject>();
                }
            }
        }

        private void HandleDieEvent()
        {
            List<EntityHealth> deadentityHealth = new List<EntityHealth>();
            foreach (EntityHealth entityHealth in _markedEntityHealthList.Keys)
            {
                if (entityHealth.IsDie)
                {
                    entityHealth.OnDieEvent -= HandleDieEvent;
                    deadentityHealth.Add(entityHealth);
                }
            }
            foreach (EntityHealth entityHealth in deadentityHealth)
            {
                if (entityHealth.IsDie)
                {
                    _markedEntityHealthList[entityHealth].Push();
                    _markedEntityHealthList.Remove(entityHealth);
                }
            }
        }

        private void MarkAttack()
        {
            List<EntityHealth> entityHealthList = _markedEntityHealthList.Keys.ToList();
            foreach (EntityHealth entityHealth in entityHealthList)
            {
                _markedEntityHealthList[entityHealth].Push();
                entityHealth.OnDieEvent -= HandleDieEvent;
                entityHealth.ApplyDamage(damage, default, null, attackType: EAttackType.Fixed);
            }
            CameraManager.Instance.ShakeCamera(10, 10, 0.2f);
        }
    }
}
