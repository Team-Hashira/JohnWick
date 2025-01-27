using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class PistolGun : GunWeapon
    {
        public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
            if (isDown == false) return;

            if (Fire() == false) return;

            CameraManager.Instance.ShakeCamera(8, 10, 0.15f);
        }

        protected override bool Fire()
        {
            if (base.Fire() == false) return false;

            Vector3 firePos = EntityWeapon.VisualTrm.position + EntityWeapon.transform.rotation * GunSO.firePoint;
            Vector3 direction = CalculateRecoil(EntityWeapon.transform.right);
            CreateBullet(firePos, direction);
            //Effect
            EntityWeapon.gameObject.Pop(GunSO.fireSpakleEffect, firePos, Quaternion.LookRotation(Vector3.back, EntityWeapon.transform.right));
            return true;
        }
    }
}
