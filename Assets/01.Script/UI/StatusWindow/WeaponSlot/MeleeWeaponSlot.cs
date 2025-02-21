using Hashira.Core.StatSystem;
using Hashira.Items;
using Hashira.Items.Weapons;
using Hashira.UI.DragSystem;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class MeleeWeaponSlot : MonoBehaviour, IWeaponSlot, ISelectableObject
    {
        [SerializeField] private WeaponSlotIcon _icon;

        public int SlotIndex { get; set; }
        public Item Item { get; set; }

        public IStatable GetStatable()
        {
            return Item as IStatable;
        }

        public void HandleWeaponChanged(Weapon weapon)
        {
            Item = weapon;
            _icon.Init(this);
        }

        public void OnSelectEnd()
        {
        }

        public void OnSelectStart()
        {
        }
    }
}
