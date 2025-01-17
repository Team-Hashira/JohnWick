using AYellowpaper.SerializedCollections;
using Hashira.Core.StatSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Weapons
{
    public class WeaponSO : ItemSO
    {
        [field: Header("==========Weapon setting==========")]
        [field: SerializeField] public LayerMask WhatIsTarget { get; internal set; }
        [Header("Parts")]
        [Tooltip("Is local position")]
        public SerializedDictionary<EWeaponPartsType, Vector2> partsEquipPosDict 
            = new SerializedDictionary<EWeaponPartsType, Vector2>();
        [Header("Stat")]
        public List<StatElement> overrideStatElementList = new List<StatElement>();
        public StatBaseSO baseStat;

        private Weapon weapon;

        private void OnEnable()
        {
            string className = name;
            try
            {
                Type type = Type.GetType("Hashira.Weapons." + className);
                Weapon findedWeapon = Activator.CreateInstance(type) as Weapon;
                findedWeapon.Init(this);
                weapon = findedWeapon;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{className} not found.\n" +
                                $"Error : {ex.ToString()}");
            }
        }

        public Weapon GetWeaponClass()
        {
            return weapon.Clone() as Weapon;
        }
    }
}
