using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class Weapon : Item, IStatable
    {
        public WeaponSO WeaponSO { get; private set; }

        //Stat
        public StatDictionary StatDictionary { get; private set; }
        public EntityWeapon EntityWeapon { get; private set; }

        private int _entityDamage;
        public LayerMask WhatIsTarget { get; private set; }

        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);
            WeaponSO = ItemSO as WeaponSO;
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

        public override object Clone()
        {
            Weapon clonedWeapon = (Weapon)base.Clone();

            if (WeaponSO.baseStat == null) Debug.LogError("BaseStat is null with WeaponSO");
            else
            {
                StatBaseSO baseStat = GameObject.Instantiate(WeaponSO.baseStat);
                List<StatElement> overrideStatElementList = new List<StatElement>();
                foreach (StatElement statElement in WeaponSO.overrideStatElementList)
                {
                    overrideStatElementList.Add((StatElement)statElement.Clone());
                }

                clonedWeapon.StatDictionary = new StatDictionary(overrideStatElementList, baseStat);
            }

            return clonedWeapon;
        }
    }
}
