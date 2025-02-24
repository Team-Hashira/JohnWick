using Hashira.Entities.Components;
using Hashira.Items.Weapons;
using Hashira.Players;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class WeaponContainer : MonoBehaviour
    {
        private ISubItemSlot _subItemSlots;
        private IWeaponSlot[] _weaponSlots;
        private Player _player;
        private EntityWeaponHolder _entityGunWeapon;
        private EntitySubItemHolder _entitySubItemWeapon;

        private int _meleeWeaponIndex;

        private void Awake()
        {
            _subItemSlots = GetComponentInChildren<ISubItemSlot>();
            _weaponSlots = GetComponentsInChildren<IWeaponSlot>();
            _player = GameManager.Instance.Player;
            
            _entityGunWeapon = _player.GetEntityComponent<EntityWeaponHolder>();
            _entitySubItemWeapon = _player.GetEntityComponent<EntitySubItemHolder>();

            SlotSetting();
        }

        private void SlotSetting()
        {
            for (var i = 0; i < _entityGunWeapon.OnChangedWeaponEvents.Length; i++)
            {
                _entityGunWeapon.OnChangedWeaponEvents[i] += _weaponSlots[i].HandleWeaponChanged;
                _weaponSlots[i].HandleWeaponChanged(_entityGunWeapon.Items[i] as Weapon);
                _weaponSlots[i].SlotIndex = i;
            }
            _meleeWeaponIndex = _entityGunWeapon.OnChangedWeaponEvents.Length;
            _entitySubItemWeapon.OnChangedSubItemEvents[0] += _subItemSlots.HandleSubItemChanged;
            _subItemSlots.HandleSubItemChanged(_entitySubItemWeapon.SubItems[0]);
            _subItemSlots.SlotIndex = _meleeWeaponIndex;
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _entityGunWeapon.OnChangedWeaponEvents.Length; i++)
                _entityGunWeapon.OnChangedWeaponEvents[i] -= _weaponSlots[i].HandleWeaponChanged;

            _entitySubItemWeapon.OnChangedSubItemEvents[0] -= _subItemSlots.HandleSubItemChanged;
        }
    }
}
