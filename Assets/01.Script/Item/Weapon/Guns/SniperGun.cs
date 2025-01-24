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

            Vector3 firePos = _EntityWeapon.VisualTrm.position + _EntityWeapon.transform.rotation * GunSO.firePoint;
            Vector3 direction = CalculateRecoil(_EntityWeapon.transform.right);
            CreateBullet(firePos, direction);
            //Effect
            _EntityWeapon.gameObject.Pop(GunSO.fireSpakleEffect, firePos, Quaternion.LookRotation(Vector3.back, _EntityWeapon.transform.right));
            return true;
        }
    }
}
