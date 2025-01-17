using System;

namespace Hashira.Items.Weapons
{
    public class GunWeapon : Weapon
    {
        public GunSO GunSO { get; private set; }

        public event Action<int> OnFireEvent;
        public int BulletAmount { get; protected set; }

        private void HandleDamageSuccessEvent()
        {
            CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
        }

        public override void Attack(int damage, bool isDown)
        {
            //???????
        }

        protected void SpawnCartridgeCase()
        {
            _EntityWeapon.CartridgeCaseParticle.Play();
        }

        protected virtual bool Fire()
        {
            //?????
            if (BulletAmount <= 0) return false;
            BulletAmount--;
            OnFireEvent?.Invoke(BulletAmount);

            SpawnCartridgeCase();

            return true;
        }

        public void Reload()
        {
            BulletAmount = GunSO.MaxBulletAmount;
        }

        public override object Clone()
        {
            GunSO = WeaponSO as GunSO;
            Reload();
            return base.Clone();
        }
    }
}