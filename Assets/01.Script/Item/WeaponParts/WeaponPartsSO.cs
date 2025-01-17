using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum EWeaponPartsType
{
    Muzzle,             //√—±∏
    Scope,              //Ω∫ƒ⁄«¡
    Grip,               //º’¿‚¿Ã 
    Magazine,           //≈∫√¢
    CartridgeBelt,      //≈∫∂Ï
    Stock,              //∞≥∏”∏Æ∆«
}

namespace Hashira.Items.WeaponPartsSystem
{
    [CreateAssetMenu(fileName = "WeaponPartsSO", menuName = "SO/Weapon/Parts")]
    public class WeaponPartsSO : ItemSO, IStatable
    { 
        [Header("==========Weapon parts setting==========")]
        public EWeaponPartsType partsType;

        private WeaponParts weaponParts;
        [SerializeField] private List<StatElement> _statList = new List<StatElement>();
        public StatDictionary StatDictionary { get; private set; }

        private void OnEnable()
        {
            string className = name;
            try
            {
                Type type = Type.GetType("Hashira.Items.WeaponPartsSystem." + className);
                WeaponParts findedWeaponParts = Activator.CreateInstance(type) as WeaponParts;
                findedWeaponParts.Init(this);
                weaponParts = findedWeaponParts;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{className} not found.\n" + 
                                $"Error : {ex.ToString()}");
            }
            StatDictionary = new StatDictionary(_statList);
        }

        public WeaponParts GetWeaponPartsClass()
        {
            return weaponParts.Clone() as WeaponParts;
        }
    }
}
