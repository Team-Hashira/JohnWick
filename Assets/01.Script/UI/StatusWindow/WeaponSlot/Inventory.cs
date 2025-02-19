using Hashira.Items.PartsSystem;
using System;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class Inventory : MonoBehaviour
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
                weaponParts[i] = slots[i].BasePart;
                Debug.Log(weaponParts[i]);
            }
            Debug.Log("----------------------------------");
        }
    }
}
