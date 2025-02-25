using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Players;

namespace Hashira.Items.Modules
{
    public class Module : Item
    {
        protected ModuleSO _moduleItemSO;
        protected EntityStat _entityStat;
        protected Attacker _attacker;
        protected Player _player;

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
            foreach (StatElementAdjustment adjustment in _moduleItemSO.StatVariationList)
            {
                _entityStat.StatDictionary[adjustment.elementSO]
                    .AddModify(_moduleItemSO.itemName, adjustment.Value, adjustment.IsPercentAdjustment ? EModifyMode.Percent : EModifyMode.Add);
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
                    .RemoveModify(_moduleItemSO.itemName, adjustment.IsPercentAdjustment ? EModifyMode.Percent : EModifyMode.Add);
            }
        }
    }
}
