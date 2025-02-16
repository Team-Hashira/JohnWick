using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class SniperGun : GunWeapon
    {
        public override void Attack(int damage, bool isDown, LayerMask whatIsTarget)
        {
            base.Attack(damage, isDown, whatIsTarget);
            if (isDown == false) return;

            if (Fire() == false) return;

            CameraManager.Instance.ShakeCamera(12, 12, 0.3f);
        }

        protected override bool Fire()
        {
            if (base.Fire() == false) return false;
            
            Vector3 direction = CalculateRecoil(EntityGunWeapon.transform.right);
            CreateBullet(direction);
            //Effect
            EntityGunWeapon.gameObject.Pop(GunSO.fireSpakleEffect, FirePos, Quaternion.LookRotation(Vector3.back, EntityGunWeapon.transform.right));
            return true;
        }
    }
}
