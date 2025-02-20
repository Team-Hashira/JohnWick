using Hashira.Items;
using TMPro;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class WeaponDescriptionContainer : MonoSingleton<WeaponDescriptionContainer>
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        private RectTransform _rectTransform;
        private Vector2 _size;

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            _size = (transform.Find("Background").transform as RectTransform).sizeDelta;
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Open(ItemSO itemSO, RectTransform posTarget)
        {
            _rectTransform.position = posTarget.position + new Vector3(0, (posTarget.anchoredPosition.y > 0 ? 0 : _size.y));

            _nameText.text = itemSO.itemName;
            _descriptionText.text = itemSO.itemDescription;
            gameObject.SetActive(true);
        }

        public void Close() 
        { 
            gameObject.SetActive(false);
        }
    }
}
