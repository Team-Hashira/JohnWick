using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using Hashira.Items.PartsSystem;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class Weapon : Item, IStatable
    {
        public WeaponSO WeaponSO { get; private set; }

        //Stat
        private List<StatElement> _overrideStatElementList = new List<StatElement>();
        private StatBaseSO _baseStat;
        public StatDictionary StatDictionary { get; private set; }
        public EntityWeapon EntityWeapon { get; private set; }

        private int _entityDamage;
        public LayerMask WhatIsTarget {  get; private set; }

        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);

            WeaponSO = itemSO as WeaponSO;
            if (WeaponSO.baseStat == null) Debug.LogError("BaseStat is null with WeaponSO");
            else
            {
                _baseStat = GameObject.Instantiate(WeaponSO.baseStat);
                _overrideStatElementList = WeaponSO.overrideStatElementList;

                StatDictionary = new StatDictionary(_overrideStatElementList, _baseStat);
            }
        }

        public virtual void Equip(EntityWeapon entityWeapon)
        {
            EntityWeapon = entityWeapon;
        }
        public virtual void WeaponUpdate()
        {
        }
        public virtual void UnEquip()
        {
            EntityWeapon = null;
        }

        public virtual void Attack(int damage, bool isDown, LayerMask whatIsTarget)
        {
            _entityDamage = damage;
            WhatIsTarget = whatIsTarget;
        }
        public virtual int CalculateDamage() { return _entityDamage + StatDictionary["AttackPower"].IntValue; }

    }
}
