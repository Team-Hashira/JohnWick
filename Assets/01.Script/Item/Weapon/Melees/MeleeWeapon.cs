using DG.Tweening;
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



        public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
			CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
			Vector3 startRot = Vector3.forward * MeleeSO.RotateMax;
			Vector3 endRot = Vector3.forward * MeleeSO.RotateMin;
            float duration = MeleeSO.AttackDuration;
            float afterDelay = MeleeSO.AttackAfterDelay;

			(EntityMeleeWeapon.DamageCaster as BoxDamageCaster2D).size = MeleeSO.AttackRangeSize;
			(EntityMeleeWeapon.DamageCaster as BoxDamageCaster2D).center = MeleeSO.AttackRangeOffset;

            if (animationSeq != null && animationSeq.IsActive()) animationSeq.Kill();
            animationSeq = DOTween.Sequence();
            animationSeq.AppendCallback(()=>EntityMeleeWeapon.transform.localEulerAngles = startRot);
            animationSeq.Append(EntityMeleeWeapon.transform.DOLocalRotate(endRot, duration).SetEase(Ease.OutCubic));
            animationSeq.JoinCallback(() => EntityMeleeWeapon.DamageCaster.CastDamage(damage));
            animationSeq.AppendInterval(afterDelay);
            animationSeq.AppendCallback(() => AttackEnd());
        }

        public override void Equip(EntityWeapon entityWeapon)
        {
            base.Equip(entityWeapon);
            EntityMeleeWeapon = entityWeapon as EntityMeleeWeapon;
        }

        public override void UnEquip()
        {
            base.UnEquip();
            EntityMeleeWeapon = null;
        }

        private void AttackEnd()
        {
            EntityGunWeapon entityGunWeapon = EntityMeleeWeapon.GunWaepon;
            entityGunWeapon.IsMeleeWeapon = false;
            entityGunWeapon.EquipWeapon(entityGunWeapon.CurrentWeapon, entityGunWeapon.WeaponIndex);
        }
        
        public override object Clone()
        {
            MeleeSO = WeaponSO as MeleeSO;
            return base.Clone();
        }
    }
}
