using Crogen.CrogenPooling;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class MarkAttackProjectileModifier : ProjectileModifier
    {
        private static Dictionary<EntityHealth, IPoolingObject> _markedEntityHealthList = new Dictionary<EntityHealth, IPoolingObject>();
        public int damage = 500;

        public override void OnCreated(Projectile projectile)
        {
            base.OnCreated(projectile);
        }

        public override void OnHitedDamageable(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnHitedDamageable(hit, damageable);
            if (damageable is EntityHealth entityHealth && entityHealth.Owner != GameManager.Instance.Player && _markedEntityHealthList.ContainsKey(entityHealth) == false)
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
            foreach (EntityHealth entityHealth in _markedEntityHealthList.Keys)
            {
                _markedEntityHealthList[entityHealth].Push();
                entityHealth.OnDieEvent -= HandleDieEvent;
                entityHealth.ApplyDamage(damage, default, null, attackType: EAttackType.Fixed);
            }
            CameraManager.Instance.ShakeCamera(10, 10, 0.2f);
        }
    }
}
