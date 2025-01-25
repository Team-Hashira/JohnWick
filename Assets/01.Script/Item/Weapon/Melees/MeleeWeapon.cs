using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class MeleeWeapon : Weapon
    {
        public MeleeSO MeleeSO { get; private set; }
        public event Action<int> OnAttackEvent;
        protected float _currentTime = 0f;
        
        private void HandleDamageSuccessEvent()
        {
            CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
        }

        public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
            AttackEnd();
        }

        private void AttackEnd()
        {
            Sequence seq = DOTween.Sequence();
            float oldPosX = _EntityWeapon.VisualTrm.localPosition.x;
            
            seq.Append(_EntityWeapon.VisualTrm.DORotate(new Vector3(0, 0, MeleeSO.RotateMax), 0.2f));
            
            // Real attack
            seq.Append(_EntityWeapon.VisualTrm.DORotate(new Vector3(0, 0, MeleeSO.RotateMin), MeleeSO.AttackDuration))
                .Join(_EntityWeapon.VisualTrm.DOLocalMoveX(MeleeSO.Stab, MeleeSO.AttackDuration));

            seq.Append(_EntityWeapon.VisualTrm.DORotate(Vector3.zero, MeleeSO.AttackAfterDelay))
                .Join(_EntityWeapon.VisualTrm.DOLocalMoveX(oldPosX, MeleeSO.AttackAfterDelay));
                
            seq.OnComplete(() =>
            {
                _EntityWeapon.WeaponChange(_EntityWeapon.OldWeaponIndex);
            });
        }
        
        public override object Clone()
        {
            MeleeSO = WeaponSO as MeleeSO;
            return base.Clone();
        }
    }
}
