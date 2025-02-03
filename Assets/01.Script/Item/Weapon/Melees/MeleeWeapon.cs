using DG.Tweening;
using Hashira.Entities.Components;
using System;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class MeleeWeapon : Weapon
    {
        public MeleeSO MeleeSO { get; private set; }
        
        public event Action<int> OnAttackEvent;
        
		public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
			CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
			Vector3 startRot = Vector3.forward * MeleeSO.RotateMax;
			Vector3 endRot = Vector3.forward * MeleeSO.RotateMin;
            float duration = MeleeSO.AttackDuration;
            float afterDelay = MeleeSO.AttackAfterDelay;

			(EntityWeapon.DamageCaster as BoxDamageCaster2D).size = MeleeSO.AttackRangeSize;
			(EntityWeapon.DamageCaster as BoxDamageCaster2D).center = MeleeSO.AttackRangeOffset;

			Sequence seq = DOTween.Sequence();
            seq.AppendCallback(()=>EntityWeapon.transform.localEulerAngles = startRot);
            seq.Append(EntityWeapon.transform.DOLocalRotate(endRot, duration).SetEase(Ease.OutCubic));
            seq.JoinCallback(() => EntityWeapon.DamageCaster.CastDamage(damage));
			seq.AppendInterval(afterDelay);
			seq.OnComplete(() => AttackEnd());
        }

        private void AttackEnd()
        {
            EntityWeapon.WeaponChange(EntityWeapon.OldWeaponIndex);
        }
        
        public override object Clone()
        {
            MeleeSO = WeaponSO as MeleeSO;
            return base.Clone();
        }
    }
}
