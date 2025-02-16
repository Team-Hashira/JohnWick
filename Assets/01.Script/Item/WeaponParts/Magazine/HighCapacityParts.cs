using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class HighCapacityParts : WeaponParts
    {
        private int _grenadeDelay = 3;
        private int _fireCount = 0;

        public override void Equip(GunWeapon weapon)
        {
            base.Equip(weapon);
            _fireCount = 0;
            _weapon.OnFireEvent += HandleFireEvent;
        }

        private void HandleFireEvent(int bulletAmount)
        {
            _fireCount++;
            if (_fireCount == _grenadeDelay)
            {
                _fireCount = 0;
                Debug.Log("히히 유탄발사~");
            }
        }

        public override void PartsUpdate()
        {
            base.PartsUpdate();
            _weapon.OnFireEvent -= HandleFireEvent;
        }

        public override void UnEquip()
        {
            base.UnEquip();
        }
    }
}
