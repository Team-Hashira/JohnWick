using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Items;
using Hashira.Players;
using Hashira.UI.DragSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class DescriptionContainer : MonoSingleton<DescriptionContainer>
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _itemTypeText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _statusNameText;
        [SerializeField] private TextMeshProUGUI _statusValueText;
        [Space(25)]
        [SerializeField] private Sprite _playerProfileIcon;

        private Player _player;
        private EntityStat _entityStat;
        
        private void Awake()
        {
            _player = GameManager.Instance.Player;
        }

        private void Start()
        {
            _entityStat = _player.GetEntityComponent<EntityStat>();
        }

        private void OnEnable()
        {
            if (UIMouseController.Instance != null)
                UIMouseController.Instance.CanSelect = true;

            if (_entityStat != null)
            {
                SetPlayerStatusProfile();
            }
        }

        private void OnDisable()
        {
            if (UIMouseController.Instance != null)
                UIMouseController.Instance.CanSelect = false;
        }

        private void LateUpdate()
        {
            ISelectableObject selectableObject = UIMouseController.Instance.CurrentSelectedObject;

            if (selectableObject is ISlot slot == false)
            {
                SetPlayerStatusProfile();
                return;
            }
            if (slot.Item == null) return;

            Item item = slot.Item;
            ItemSO itemSO = item.ItemSO;

            _iconImage.sprite = itemSO.itemIcon;
            _itemNameText.text = itemSO.itemDisplayName;
            _descriptionText.text = itemSO.itemDescription;

            StatElement[] statElements = item.StatDictionary.GetElements();
            SetStatusText(statElements);
        }

        private void SetStatusText(StatElement[] statElements)
        {
            _statusNameText.text = string.Empty;
            _statusValueText.text = string.Empty;

            foreach (var statElement in statElements)
            {
                _statusNameText.text += $"{statElement.elementSO.displayName}\n";
                _statusValueText.text += $"{statElement.Value}\n";
            }
        }

        private void SetPlayerStatusProfile()
        {
            _iconImage.sprite = _playerProfileIcon;
            _itemNameText.text = "할머니";
            _descriptionText.text = "평범한 할머니다.";

            StatElement[] statElements = _entityStat.StatDictionary.GetElements();
            SetStatusText(statElements);
        }
    }
}
