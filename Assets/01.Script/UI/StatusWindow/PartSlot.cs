using Hashira.Items.PartsSystem;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class PartSlot : MonoBehaviour
    {
        [SerializeField] private PartSlotIcon _icon;
        public EWeaponPartsType partType;
        public WeaponParts BasePart { get; private set; }
        public GunWeaponSlot Parent { get; private set; }
        
        public void Init(GunWeaponSlot weaponSlot, WeaponParts weaponPart)
        {
            Parent = weaponSlot;
            BasePart = weaponPart;
            _icon.Init(this);
        }
    }
}
