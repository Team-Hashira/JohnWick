using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class PistolGun : GunWeapon
    {
        public override void Attack(int damage, bool isDown, LayerMask whatIsTarget)
        {
            base.Attack(damage, isDown, whatIsTarget);
            if (isDown == false) return;

            if (Fire() == false) return;

            CameraManager.Instance.ShakeCamera(8, 10, 0.15f);
        }

        protected override bool Fire()
        {
            if (base.Fire() == false) return false;

            Vector3 direction = CalculateRecoil(EntityGunWeapon.transform.right);
            CreateBullet(direction);
            //Effect
            EntityGunWeapon.gameObject.Pop(GunSO.fireSpakleEffect, _firePos, Quaternion.LookRotation(Vector3.back, EntityGunWeapon.transform.right));
            return true;
        }
    }
}
