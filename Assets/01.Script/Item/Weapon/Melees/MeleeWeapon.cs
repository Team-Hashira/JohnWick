using DG.Tweening;
using Hashira.Combat;
using Hashira.Entities;
using Hashira.Entities.Components;
using System;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class MeleeWeapon : Weapon
    {
        public MeleeSO MeleeSO { get; private set; }
        public EntityMeleeWeapon EntityMeleeWeapon { get; private set; }

        public event Action<int> OnAttackEvent;

        private Sequence animationSeq;

        private bool _onParrying;
        private bool _isCharged;

        public override void Attack(int damage, bool isDown, LayerMask whatIsTarget)
        {
            base.Attack(damage, isDown, whatIsTarget);

			Vector3 startRot = Vector3.forward * MeleeSO.RotateMax;
			Vector3 endRot = Vector3.forward * MeleeSO.RotateMin;
            float duration = MeleeSO.AttackDuration;
            float afterDelay = MeleeSO.AttackAfterDelay;

            _onParrying = true;
            _isCharged = EntityMeleeWeapon.IsCharged;

            (EntityMeleeWeapon.DamageCaster as BoxDamageCaster2D).size = MeleeSO.AttackRangeSize;
			(EntityMeleeWeapon.DamageCaster as BoxDamageCaster2D).center = MeleeSO.AttackRangeOffset;

            Vector2 attackDir = (EntityMeleeWeapon.DamageCaster.transform.position - EntityMeleeWeapon.Entity.transform.position).normalized;

            if (animationSeq != null && animationSeq.IsActive()) animationSeq.Kill();
            animationSeq = DOTween.Sequence();
            animationSeq.AppendCallback(() => EntityMeleeWeapon.transform.localEulerAngles = startRot);
            animationSeq.Append(EntityMeleeWeapon.transform.DOLocalRotate(endRot, duration).SetEase(Ease.OutBack));
            animationSeq.InsertCallback(duration / 5, () => EntityMeleeWeapon.DamageCaster.CastDamage(damage, knockback: attackDir * 15f));
            animationSeq.InsertCallback(duration / 2, () =>
            {
                _onParrying = false;
                _isCharged = false;
            });
            animationSeq.AppendInterval(afterDelay);
            animationSeq.AppendCallback(() => AttackEnd());
        }

        public override void Equip(EntityWeapon entityWeapon)
        {
            base.Equip(entityWeapon);
            EntityMeleeWeapon = entityWeapon as EntityMeleeWeapon;
        }

        public override void WeaponUpdate()
        {
            base.WeaponUpdate();
            if (_onParrying)
            {
                RaycastHit2D[] hits = EntityMeleeWeapon.DamageCaster.CastOverlap();
                foreach (var hit in hits)
                {
                    Transform owner = EntityMeleeWeapon.Entity.transform;
                    if (hit.transform.TryGetComponent(out IParryingable parryingable) && parryingable.Owner != owner)
                        parryingable.Parrying(_whatIsTarget, owner, _isCharged);
                }
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();
            EntityMeleeWeapon = null;
        }

        private void AttackEnd()
        {
            EntityGunWeapon entityGunWeapon = EntityMeleeWeapon.GunWaepon;
            entityGunWeapon.IsMeleeWeaponMode = false;
            entityGunWeapon.EquipWeapon(entityGunWeapon.CurrentWeapon, entityGunWeapon.CurrentIndex);
        }
        
        public override object Clone()
        {
            MeleeSO = WeaponSO as MeleeSO;
            return base.Clone();
        }
    }
}
