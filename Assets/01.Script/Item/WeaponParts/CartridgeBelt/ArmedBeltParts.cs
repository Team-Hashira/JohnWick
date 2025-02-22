using Hashira.Items.Weapons;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    //무장탄띠 기능 보류
    public class ArmedBeltParts : WeaponParts
    {
        private GunStatIncreaseProjectileModifier _gunStatIncreaseProjectileModifier;

        public override object Clone()
        {
            _gunStatIncreaseProjectileModifier = new GunStatIncreaseProjectileModifier();
            
            return base.Clone();
        }

        public override void Equip(GunWeapon weapon)
        {
            base.Equip(weapon);
            _weapon.AddProjectileModifier(_gunStatIncreaseProjectileModifier);
        }

        public override void UnEquip()
        {
            base.UnEquip();
            _weapon.RemoveProjectileModifier(_gunStatIncreaseProjectileModifier);
        }
    }
}
