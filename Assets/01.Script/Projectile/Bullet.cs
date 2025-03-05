using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Bullet : Projectile, IParryingable
    {
        public bool IsParryingable { get; set; }
        [SerializeField] protected EffectPoolType _hitEffect;
        [SerializeField] protected EffectPoolType _spakleEffect;

        public override void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner, List<ProjectileModifier> projectileModifiers = default, AnimationCurve damageOverDistance = null)
        {
            base.Init(whatIsTarget, direction, speed, damage, penetration, owner, projectileModifiers, damageOverDistance);
            Owner = owner;
            IsParryingable = true;
        }

        protected override void OnHited(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnHited(hit, damageable);
            if (damageable != null)
            {
                int damage = CalculateDamage(Damage);
                int penetrationDamage = CalculatePenetration(CalculateDamage(damage), _penetration - _currentPenetration);
                EEntityPartType parts = damageable.ApplyDamage(penetrationDamage, hit, transform, transform.right * 4, _attackType);

                if (damageable is EntityHealth health && health.TryGetComponent(out Entity entity))
                {
                    // TODO 데미지 입혀야 함
                }
            }
            else
            {
                //Effect
                gameObject.Pop(_spakleEffect, hit.point + hit.normal * 0.1f, Quaternion.LookRotation(Vector3.back, hit.normal));
            }
        }

        public void Parrying(LayerMask whatIsNewTargetLayer, Transform owner, bool isChargedParrying)
        {
            if (IsParryingable == false) return;

            Quaternion effectRotation = transform.rotation * Quaternion.Euler(0, 0, -90);
            gameObject.Pop(EffectPoolType.HitSparkleEffect, transform.position, effectRotation);

            if (isChargedParrying)
            {
                CameraManager.Instance.ShakeCamera(15, 11, 0.25f);
                Damage *= 10;
                _speed *= 10;
                gameObject.Pop(EffectPoolType.HitSparkleEffect, transform.position, effectRotation);
            }
            else
                CameraManager.Instance.ShakeCamera(4, 4, 0.15f);

            WhatIsTarget = whatIsNewTargetLayer;
            IsParryingable = false;
            Owner = owner;
            transform.localEulerAngles += new Vector3(0, 180, 0);
        }
    }
}