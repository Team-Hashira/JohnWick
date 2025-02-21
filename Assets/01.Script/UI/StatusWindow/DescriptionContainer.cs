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
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _statusNameText;
        [SerializeField] private TextMeshProUGUI _statusValueText;
        
        private Player _player;
        private EntityStat _entityStat;
        
        private StatElement[] _statElement;

        private void Awake()
        {
            _player = GameManager.Instance.Player;
        }

        private void Start()
        {
            _entityStat = _player.GetEntityComponent<EntityStat>();
        }

        private void LateUpdate()
        {
            ISelectableObject selectableObject = UIMouseController.Instance.GetCurrentSelectedObject();

            if (selectableObject is ISlot slot == false) return;
            if (slot.Item == null) return;

            ItemSO itemSO = slot.Item.ItemSO;

            _iconImage.sprite = itemSO.itemIcon;
            _itemNameText.text = itemSO.itemDisplayName;
            _descriptionText.text = itemSO.itemDescription;

            StatElement[] statElements = _entityStat.StatDictionary.GetElements();
            _statusNameText.text = string.Empty;
            _statusValueText.text = string.Empty;

            foreach (var statElement in statElements)
            {
                _statusNameText.text += $"{statElement.elementSO.displayName}\n";
                _statusValueText.text += $"{statElement.Value}\n";
            }
        }
    }
}
