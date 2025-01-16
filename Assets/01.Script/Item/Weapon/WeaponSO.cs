using Hashira.Core.StatSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Weapons
{
    public class WeaponSO : ItemSO
    {
        [Header("Weapon setting")]
        public EWeaponPartsType equippableWeaponPartsType;
        public List<StatElement> statElementLsit = new List<StatElement>();
        [field: SerializeField] public LayerMask WhatIsTarget { get; internal set; }
        public StatBaseSO baseStat;

        private Weapon weapon;

        private void OnEnable()
        {
            string className = name.Replace("WeaponSO", "");
            Type type = Type.GetType("Hashira.Weapons." + className);
            Weapon newWeapon = Activator.CreateInstance(type) as Weapon;
            newWeapon.Init(this);
            weapon = newWeapon;
        }

        public Weapon GetWeaponClass()
        {
            return weapon.Clone() as Weapon;
        }
    }
}
