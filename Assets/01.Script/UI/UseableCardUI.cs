using Crogen.CrogenPooling;
using Hashira.Cards;
using Hashira.Core;
using Hashira.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.LatestUI
{
    public class UseableCardUI : UIBase, IClickableUI, IHoverableUI, IPoolingObject
    {
        [field: SerializeField] public Collider2D Collider { get; set; }
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private CardSO _cardSO;

        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _icon;

        private void Start()
        {
            UIManager.Instance.AddUI(this);
        }

        public void SetCard(CardSO cardSO)
        {
            _cardSO = cardSO;
            _description.text = cardSO.cardDescription;
            _icon.sprite = cardSO.iconSprite;
        }

        public void OnClick()
        {
            if (Cost.TryRemoveCost(_cardSO.needCost))
            {
                PlayerManager.Instance.Player.GetEntityComponent<EntityEffector>().AddEffect(_cardSO.GetEffectClass());
                this.Push();
            }
            else
            {
                Debug.Log("Cost가 부족해요");
            }
        }

        public void OnClickEnd()
        {

        }

        public void OnCursorEnter()
        {
            transform.localScale = Vector3.one * 1.1f;
        }

        public void OnCursorExit()
        {
            transform.localScale = Vector3.one;
        }

        public void OnPop()
        {
            transform.localScale = Vector3.one;
        }

        public void OnPush()
        {

        }
    }
}
