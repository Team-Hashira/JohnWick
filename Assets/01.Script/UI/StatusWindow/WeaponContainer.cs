using Hashira.Entities.Components;
using Hashira.Players;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class WeaponContainer : MonoBehaviour
    {
        private IWeaponSlot[] _slots;
        private Player _player;
        private EntityGunWeapon _entityGunWeapon;
        private EntityMeleeWeapon _entityMeleeWeapon;

        private int _meleeWeaponIndex;

        private void Awake()
        {
            _slots = GetComponentsInChildren<IWeaponSlot>();
            _player = GameManager.Instance.Player;
            
            _entityGunWeapon = _player.GetEntityComponent<EntityGunWeapon>();
            _entityMeleeWeapon = _player.GetEntityComponent<EntityMeleeWeapon>();
        }

        private void Start()
        {
            for (var i = 0; i < _entityGunWeapon.OnChangedWeaponEvents.Length; i++)
            {
                _entityGunWeapon.OnChangedWeaponEvents[i] += _slots[i].HandleWeaponChanged;
                _slots[i].HandleWeaponChanged(_entityGunWeapon.Weapons[i]);
                _slots[i].SlotIndex = i;
            }
            _meleeWeaponIndex = _entityGunWeapon.OnChangedWeaponEvents.Length;
            _entityMeleeWeapon.OnChangedWeaponEvents[0] += _slots[_meleeWeaponIndex].HandleWeaponChanged;
            _slots[_meleeWeaponIndex].HandleWeaponChanged(_entityMeleeWeapon.Weapons[0]);
            _slots[_meleeWeaponIndex].SlotIndex = _meleeWeaponIndex;
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _entityGunWeapon.OnChangedWeaponEvents.Length; i++)
                _entityGunWeapon.OnChangedWeaponEvents[i] -= _slots[i].HandleWeaponChanged;

            _entityMeleeWeapon.OnChangedWeaponEvents[0] -= _slots[_meleeWeaponIndex].HandleWeaponChanged;
        }
    }
}
