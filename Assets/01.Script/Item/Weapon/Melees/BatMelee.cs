using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities.Components;
using System;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class BatMelee : MeleeWeapon
    {
        private int _damage;


        public override void Equip(EntityWeapon entityWeapon)
        {
            base.Equip(entityWeapon);
            EntityMeleeWeapon.DamageCaster.OnDamageCastSuccessEvent += HandleDamageCastSuccessEvent;
            EntityMeleeWeapon.DamageCaster.OnCasterSuccessEvent += HandleCasterSuccessEvent;
        }

        private void HandleCasterSuccessEvent(RaycastHit2D hit)
        {
            if (hit.transform.TryGetComponent(out IParryingable parryingable))
                parryingable.Parrying(_whatIsTarget, EntityMeleeWeapon.Entity.transform);
        }

        private void HandleDamageCastSuccessEvent(RaycastHit2D hit)
        {
            CameraManager.Instance.ShakeCamera(10, 10, 0.2f);

            //Effect
            ParticleSystem wallBloodEffect = EntityMeleeWeapon.gameObject.Pop(EffectPoolType.SpreadWallBlood, hit.point, EntityMeleeWeapon.transform.parent.rotation)
                .gameObject.GetComponent<ParticleSystem>();
            var limitVelocityOverLifetimeModule = wallBloodEffect.limitVelocityOverLifetime;
            limitVelocityOverLifetimeModule.dampen = 0.9f;

            //Effect
            ParticleSystem bloodBackEffect = EntityMeleeWeapon.gameObject.Pop(EffectPoolType.HitBloodBack, hit.point, EntityMeleeWeapon.transform.parent.rotation)
                .gameObject.GetComponent<ParticleSystem>();
        }

        public override void Attack(int damage, bool isDown, LayerMask whatIsTarget)
        {
            base.Attack(damage, isDown, whatIsTarget);

            //휘두르는 이펙트?
        }

        public override void UnEquip()
        {
            base.UnEquip();
            EntityMeleeWeapon.DamageCaster.OnDamageCastSuccessEvent -= HandleDamageCastSuccessEvent;
        }
    }
}