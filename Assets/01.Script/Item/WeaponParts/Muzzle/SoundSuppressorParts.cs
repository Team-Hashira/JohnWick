using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.Items.WeaponPartsSystem
{
    public class SoundSuppressorParts : WeaponParts
    {
        public override void Equip(Weapon weapon)
        {
            base.Equip(weapon);
            _weapon.StatDictionary["AttackPower"].AddModify(WeaponPartsSO.itemName, 10, Core.StatSystem.EModifyMode.Add);
            Debug.Log($"√— ∞≠»≠! {_weapon.StatDictionary["AttackPower"].IntValue}");
        }

        public override void PartsUpdate()
        {
            base.PartsUpdate();
        }

        public override void UnEquip()
        {
            base.UnEquip();
            _weapon.StatDictionary["AttackPower"].RemoveModify(WeaponPartsSO.itemName, Core.StatSystem.EModifyMode.Add);
            Debug.Log($"√— ∞≠»≠«ÿ¡¶! {_weapon.StatDictionary["AttackPower"].IntValue}");
        }
    }
}
