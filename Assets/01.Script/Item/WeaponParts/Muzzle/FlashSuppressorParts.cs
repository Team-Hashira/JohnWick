using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class FlashSuppressorParts : WeaponParts
    {
        private IgnitionProjectileModifier _ignitionProjectileModifier;

        public override object Clone()
        {
            _ignitionProjectileModifier = new IgnitionProjectileModifier();
            _ignitionProjectileModifier.ignitionLevel = 1;
            _ignitionProjectileModifier.ignitionDuration = 5;
            return base.Clone();
        }

        public override void Equip(GunWeapon weapon)
        {
            base.Equip(weapon);
            _weapon.AddProjectileModifier(_ignitionProjectileModifier);
        }

        public override void UnEquip()
        {
            base.UnEquip();
            _weapon.RemoveProjectileModifier(_ignitionProjectileModifier);
        }
    }
}
