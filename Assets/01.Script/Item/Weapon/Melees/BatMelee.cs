using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class BatMelee : MeleeWeapon
    {
        private int _damage;

        public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
            Debug.Log("이거 근접 공격하는 고에요");
            CameraManager.Instance.ShakeCamera(8, 10, 0.15f);
        }
    }
}