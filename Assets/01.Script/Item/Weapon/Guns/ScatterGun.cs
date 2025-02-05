using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class ScatterGun : GunWeapon
    {
        private static int _bulletCount = 5;
        private static int _scatterAngle = 5;

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

            Vector3 direction = CalculateRecoil(EntityWeapon.transform.right);
            for (int i = 0; i < _bulletCount; i++)
            {
                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(-_scatterAngle, _scatterAngle));
                CreateBullet(randomRotation * direction);
            }
            //Effect
            EntityWeapon.gameObject.Pop(GunSO.fireSpakleEffect, _firePos, Quaternion.LookRotation(Vector3.back, EntityWeapon.transform.right));
            return true;
        }

        public override int CalculateDamage()
        {
            return base.CalculateDamage() / _bulletCount;
        }
    }
}
