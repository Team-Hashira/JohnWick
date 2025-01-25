using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class MeleeWeapon : Weapon
    {
        public MeleeSO MeleeSO { get; private set; }
        
        public event Action<int> OnAttackEvent;
        
        private void HandleDamageSuccessEvent()
        {
            CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
        }

        public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
            
            Debug.Log("1초 기다리고");
            AttackEnd();
        }

        private async void AttackEnd()
        {
            await Task.Delay(1000);
            EntityWeapon.WeaponChange(EntityWeapon.OldWeaponIndex);
        }
        
        public override object Clone()
        {
            MeleeSO = WeaponSO as MeleeSO;
            return base.Clone();
        }
    }
}
