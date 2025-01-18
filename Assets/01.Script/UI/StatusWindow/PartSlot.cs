using Hashira.Items.WeaponPartsSystem;
using Hashira.UI.DragSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class PartSlot : MonoBehaviour, IDraggableObject
    {
        [SerializeField] private Image _iconImage;
        private Button _button; 
        public EWeaponPartsType partType;
        public WeaponParts BasePart { get; private set; }
        public Vector2 DragPosition { get; set; }
        public RectTransform RectTransform { get; set; }

        private void Awake()
        {
            RectTransform = transform as RectTransform;
            _button = GetComponent<Button>();
        }

        public void Init(WeaponParts weaponPart)
        {
            BasePart = weaponPart;
            _iconImage.sprite = weaponPart?.WeaponPartsSO.itemSprite;
            _iconImage.color = weaponPart != null ? Color.white : Color.clear;
        }

        private void ThrowOut()
        {
            
        }
        
        public void OnDragStart()
        {
            
        }

        public void OnDragging(Vector2 curPos)
        {
            
        }

        public void OnDragEnd(Vector2 curPos)
        {
            
        }
    }
}
