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
        [field: SerializeField] public Vector3 GrapOffset { get; internal set; }
        [field: SerializeField] public Vector3 RightHandOffset { get; internal set; }
        [field: SerializeField] public Vector3 LeftHandOffset { get; internal set; }
        [field: SerializeField] public float GrapRotate { get; internal set; }
        [Header("Parts")]
        [Tooltip("Is local position")]
        public SerializedDictionary<EWeaponPartsType, Vector2> partsEquipPosDict 
            = new SerializedDictionary<EWeaponPartsType, Vector2>();
        [Header("Stat")]
        public List<StatElement> overrideStatElementList = new List<StatElement>();
        public StatBaseSO baseStat;

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
                _itemClass = foundWeapon;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{className} not found.\n" +
                                $"Error : {ex.ToString()}");
            }
        }
    }
}
