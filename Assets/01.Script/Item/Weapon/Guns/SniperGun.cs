using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class SniperGun : GunWeapon
    {
        public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
            if (isDown == false) return;

            if (Fire() == false) return;

            CameraManager.Instance.ShakeCamera(12, 12, 0.3f);
        }

        protected override bool Fire()
        {
            if (base.Fire() == false) return false;
            
            Vector3 direction = CalculateRecoil(EntityWeapon.transform.right);
            CreateBullet(direction);
            //Effect
            EntityWeapon.gameObject.Pop(GunSO.fireSpakleEffect, _firePos, Quaternion.LookRotation(Vector3.back, EntityWeapon.transform.right));
            return true;
        }
    }
}
