using Hashira.Items.WeaponPartsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class PartSlot : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        public EWeaponPartsType partType;
        
        public void Init(WeaponPartsSO weaponPart)
        {
            _iconImage.sprite = weaponPart?.itemSprite;
            _iconImage.color = weaponPart != null ? Color.white : Color.clear;
        }
    }
}
