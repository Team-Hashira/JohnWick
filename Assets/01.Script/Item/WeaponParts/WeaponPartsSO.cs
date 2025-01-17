using Hashira.Core.StatSystem;
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
        [Header("==========Weapon parts setting==========")]
        public EWeaponPartsType partsType;

        private WeaponParts weaponParts;

        private void OnEnable()
        {
            string className = name;
            try
            {
                Type type = Type.GetType("Hashira.Weapons." + className);
                WeaponParts findedWeaponParts = Activator.CreateInstance(type) as WeaponParts;
                findedWeaponParts.Init(this);
                weaponParts = findedWeaponParts;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{className} not found.\n" + 
                                $"Error : {ex.ToString()}");
            }
        }

        public WeaponParts GetWeaponPartsClass()
        {
            return weaponParts.Clone() as WeaponParts;
        }
    }
}
