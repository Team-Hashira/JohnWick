using Crogen.CrogenPooling;
using Hashira.Cards;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.LatestUI
{
    public class BookCardUI : UIBase, IHoverableUI, IClickableUI, IPoolingObject
    {
        [field: SerializeField]
        public Collider2D Collider { get; set; }
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        [SerializeField]
        private Image _cardImage;
        [SerializeField]
        private TextMeshProUGUI _descriptionText;

        private CardSO _cardSO;

        public OnClickEvent OnClickEvent;

        private void Start()
        {
            UIManager.Instance.AddUI(this);
        }

        public void Initialize(CardSO cardSO)
        {
            _cardSO = cardSO;
            _cardImage.sprite = _cardSO.iconSprite;
            _descriptionText.text = cardSO.cardDescription;
        }

        public void OnClick()
        {
            OnClickEvent?.Invoke();
            CardBookInfoPanel panel = UIManager.Instance.GetDomain<ToggleDomain>(typeof(IToggleUI)).OpenUI("CardBookInfoPanel") as CardBookInfoPanel;
            panel.SetInfo(_cardSO);
        }

        public void OnClickEnd()
        {
        }

        public void OnCursorEnter()
        {
        }

        public void OnCursorExit()
        {
        }

        public void OnPop()
        {
        }

        public void OnPush()
        {
        }
    }
}
