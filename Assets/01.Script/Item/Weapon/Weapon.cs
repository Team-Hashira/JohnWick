using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public class Weapon : Item
    {
        public WeaponSO WeaponSO { get; private set; }

        public EntityWeaponHolder EntityWeaponHolder { get; private set; }

        public float currentCoolTime = 0;
        public bool CanSwap => currentCoolTime > WeaponSO.SwapCoolTime;

        private int _entityDamage;
        public LayerMask WhatIsTarget { get; private set; }

        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);
            WeaponSO = ItemSO as WeaponSO;
        }

        public override void Equip(EntityItemHolder entityWeapon)
        {
            EntityWeaponHolder = entityWeapon as EntityWeaponHolder;
        }
        public override void ItemUpdate()
        {
            if (EntityWeapon.WeaponCount() > 1)
                currentCoolTime += Time.deltaTime;
        }
        public override void UnEquip()
        {
            EntityWeaponHolder = null;
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
