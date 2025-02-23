using Hashira.Core.StatSystem;
using Hashira.Items;
using Hashira.Items.SubItems;
using Hashira.Items.Weapons;
using Hashira.UI.DragSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class MeleeWeaponSlot : MonoBehaviour, ISubItemSlot, ISelectableObject
    {
        [SerializeField] private WeaponSlotIcon _icon;

        public int SlotIndex { get; set; }
        public Item Item { get; set; }
        private Outline _outline;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
        }

        public void HandleSubItemChanged(SubItem weapon)
        {
            Item = weapon;
            //_icon.Init(this);
        }

        public void OnSelectStart()
        {
            _outline.enabled = true;
        }

        public void OnSelectEnd()
        {
            _outline.enabled = false;
        }
    }
}
