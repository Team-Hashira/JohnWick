using Crogen.CrogenPooling;
using Hashira.Items.Modules;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class SpawnModule : Module
    {
        [SerializeField] private float _cooldown;
        [SerializeField] private float _cooldownStartDelay;
        [SerializeField] private OtherPoolType _poolingObject;

        public override void Equip(Player player)
        {
            base.Equip(player);
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();

            if (CooldownUtillity.CheckCooldown("SpawnModule", _cooldown, true))
            {
                _player.gameObject.Pop(_poolingObject, _player.transform.position, Quaternion.identity);
                CooldownUtillity.StartCooldown("SpawnModuleDelay");
            }

            if (CooldownUtillity.CheckCooldown("SpawnModuleDelay", _cooldownStartDelay, true))
                CooldownUtillity.StartCooldown("SpawnModule");
        }

        public override void UnEquip()
        {
            base.UnEquip();
        }
    }
}
