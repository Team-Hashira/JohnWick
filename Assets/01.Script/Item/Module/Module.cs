using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Players;
using Hashira.Projectiles;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class Module : Item
    {
        protected ModuleSO _moduleItemSO;
        protected EntityStat _entityStat;
        protected Attacker _attacker;
        protected Player _player;

        public List<ProjectileModifier> ProjectileModifierList {  get; private set; }


        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);
            _moduleItemSO = itemSO as ModuleSO;
        }

        public override void Equip(Player player)
        {
            _player = player;
            _entityStat = player.GetEntityComponent<EntityStat>();
            _attacker = player.Attacker;

            ProjectileModifierList = new List<ProjectileModifier>();
            foreach (var modifierSetting in _moduleItemSO.ModifierList)
            {
                Debug.Log(modifierSetting);
                Debug.Log(modifierSetting.projectileModifierSO);
                ModifierExecuter modifierExecuter = new ModifierExecuter();
                modifierExecuter.Init(_attacker, modifierSetting, this);
                modifierSetting.projectileModifier.OnEquip(_attacker, modifierExecuter);
                ProjectileModifierList.Add(modifierSetting.projectileModifier);
            }

            foreach (StatElementAdjustment adjustment in _moduleItemSO.StatVariationList)
            {
                _entityStat.StatDictionary[adjustment.elementSO]
                    .AddModify(_moduleItemSO.itemName, adjustment.Value, adjustment.IsPercentAdjustment ? EModifyMode.Percent : EModifyMode.Add);
            }
        }

        public override void ItemUpdate()
        {
            for (int i = 0; i < ProjectileModifierList.Count; i++)
            {
                if (ProjectileModifierList[i].ModifierExecuter.CheckCondition())
                {
                    ProjectileModifierList[i].OnUpdate();
                }
            }
        }

        public override void UnEquip()
        {
            foreach (StatElementAdjustment adjustment in _moduleItemSO.StatVariationList)
            {
                _entityStat.StatDictionary[adjustment.elementSO]
                    .RemoveModify(_moduleItemSO.itemName, adjustment.IsPercentAdjustment ? EModifyMode.Percent : EModifyMode.Add);
            }
        }
    }
}
