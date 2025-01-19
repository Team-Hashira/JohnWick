using AYellowpaper.SerializedCollections;
using Hashira.Core.StatSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public abstract class WeaponSO : ItemSO
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

        private void OnValidate()
        {
            for (int i = 0; i < overrideStatElementList.Count; i++)
            {
                overrideStatElementList[i].Name = overrideStatElementList[i].elementSO.displayName;
            }
        }

        private void OnEnable()
        {
            string className = name;
            try
            {
                Type type = Type.GetType("Hashira.Items.Weapons." + className);
                Weapon foundWeapon = Activator.CreateInstance(type) as Weapon;
                foundWeapon.Init(this);
                weapon = foundWeapon;
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
