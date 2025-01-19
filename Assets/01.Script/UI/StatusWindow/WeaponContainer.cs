using Hashira.Entities.Components;
using Hashira.Players;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class WeaponContainer : MonoBehaviour
    {
        private IWeaponSlot[] _slots;
        private Player _player;
        private EntityWeapon _entityWeapon;
            
        private void Awake()
        {
            _slots = GetComponentsInChildren<IWeaponSlot>();
            _player = GameManager.Instance.Player;
            
            _entityWeapon = _player.GetEntityComponent<EntityWeapon>();

            for (var i = 0; i < _entityWeapon.OnChangedWeaponEvents.Length; i++)
            {
                _entityWeapon.OnChangedWeaponEvents[i] += _slots[i].HandleWeaponChanged;
                _slots[i].SlotIndex = i;
            }
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _entityWeapon.OnChangedWeaponEvents.Length; i++)
                _entityWeapon.OnChangedWeaponEvents[i] -= _slots[i].HandleWeaponChanged;
        }
    }
}
