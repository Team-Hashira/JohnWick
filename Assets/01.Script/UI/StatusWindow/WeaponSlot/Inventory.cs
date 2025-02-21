using Hashira.Items.PartsSystem;

namespace Hashira.UI.StatusWindow
{
    public class Inventory : MonoSingleton<Inventory>
    {
        private PartSlot[] slots;
        private WeaponParts[] weaponParts = new WeaponParts[20];

        private void Awake()
        {
            slots = GetComponentsInChildren<PartSlot>();
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].isAllType = true;
                slots[i].Init(null, null);
                slots[i].OnChangedPartsEvent += HandleSlotChanged;
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < slots.Length; i++)
                slots[i].OnChangedPartsEvent -= HandleSlotChanged;
        }

        private void HandleSlotChanged()
        {
            for (int i = 0;i < weaponParts.Length; i++)
            {
                weaponParts[i] = slots[i].Item as WeaponParts;
            }
        }

        public bool AddPart(WeaponParts part)
        {
            for (int i = 0; i < weaponParts.Length; i++)
            {
                if (weaponParts[i] == null)
                {
                    slots[i].EquipParts(part.WeaponPartsSO.partsType, part);
                    return true;
                }
            }

            return false;
        }    
    }
}
