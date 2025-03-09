using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using Hashira.MainScreen;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Bullet : Projectile
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

        protected override void OnHited(HitInfo hitInfo)
        {
            base.OnHited(hitInfo);

            var damageable = hitInfo.damageable;
            var hit = hitInfo.raycastHit;

            if (damageable != null)
            {
                MainScreenEffect.OnShake(0.45f, 12, 0.1f);

                if (damageable is EntityHealth health && health.TryGetComponent(out Entity entity))
                {
                    gameObject.Pop(EffectPoolType.BulletHitEffect, hit.point + hit.normal * 0.1f, Quaternion.LookRotation(Vector3.back, -hit.normal));
                    gameObject.Pop(EffectPoolType.HitBlood, hit.point + hit.normal * 0.1f, Quaternion.LookRotation(Vector3.back, hit.normal));
                    // TODO 데미지 입혀야 함
                }
            }
            else
            {
                //Effect
                gameObject.Pop(_spakleEffect, hit.point + hit.normal * 0.1f, Quaternion.LookRotation(Vector3.back, hit.normal));
            }
        }
    }
}