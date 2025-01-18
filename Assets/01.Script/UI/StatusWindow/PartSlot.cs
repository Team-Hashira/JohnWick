using Hashira.Items.WeaponPartsSystem;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class PartSlot : MonoBehaviour
    {
        [SerializeField] private PartSlotIcon _icon;
        public EWeaponPartsType partType;
        public WeaponParts BasePart { get; private set; }

        public void Init(WeaponParts weaponPart)
        {
            Debug.Log(weaponPart.WeaponPartsSO.name);
            BasePart = weaponPart;
            _icon.Init(this, weaponPart);
        }
    }
}
