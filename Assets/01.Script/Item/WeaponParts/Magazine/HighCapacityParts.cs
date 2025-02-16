using Crogen.CrogenPooling;
using Hashira.Items.Weapons;
using Hashira.Projectile;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class HighCapacityParts : WeaponParts
    {
        private int _grenadeDelay = 3;
        private int _fireCount = 0;
        private float _speed = 100;
        private int _damage = 1000;

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
                Debug.Log("히히 유탄발사~!");
                Grenade grenade = transform.gameObject.Pop(ProjectilePoolType.Grenade, transform.position, transform.rotation) as Grenade;
                grenade.Init(_weapon.WhatIsTarget, transform.right, _speed, _damage, 0, transform);
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();
            _weapon.OnFireEvent -= HandleFireEvent;
        }
    }
}
