using System;
using UnityEngine;

[Flags]
public enum EWeaponPartsType
{
    Muzzle = 1,             //√—±∏
    Scope = 2,              //Ω∫ƒ⁄«¡
    Grip = 4,               //º’¿‚¿Ã
    Magazine = 8,           //≈∫√¢
    CartridgeBelt = 16,     //≈∫∂Ï
    Stock = 32,             //∞≥∏”∏Æ∆«
}

namespace Hashira.Weapons
{
    [CreateAssetMenu(fileName = "WeaponPartsSO", menuName = "SO/Weapon/Parts")]
    public class WeaponPartsSO : ItemSO
    {
        [Header("Weapon parts setting")]
        public EWeaponPartsType weaponPartsType;
    }
}
