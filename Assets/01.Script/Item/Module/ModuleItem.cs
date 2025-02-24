using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Items.Module
{
    public class ModuleItem : Item
    {
        protected ModuleItemSO _moduleItemSO;
        protected EntityStat _entityStat;

        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);
            _moduleItemSO = itemSO as ModuleItemSO;
        }

        public override void Equip(Player player)
        {
            _entityStat = player.GetEntityComponent<EntityStat>();
            foreach (StatElementAdjustment adjustment in _moduleItemSO.StatVariationList)
            {
                _entityStat.StatDictionary[adjustment.elementSO]
                    .AddModify(_moduleItemSO.itemName, adjustment.Value, adjustment.IsPercentAdjustment ? EModifyMode.Percnet : EModifyMode.Add);
            }
        }

        public override void ItemUpdate()
        {

        }

        public override void UnEquip()
        {
            foreach (StatElementAdjustment adjustment in _moduleItemSO.StatVariationList)
            {
                _entityStat.StatDictionary[adjustment.elementSO]
                    .RemoveModify(_moduleItemSO.itemName, adjustment.IsPercentAdjustment ? EModifyMode.Percnet : EModifyMode.Add);
            }
        }
    }
}
