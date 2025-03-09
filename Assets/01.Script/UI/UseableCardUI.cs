using Crogen.CrogenPooling;
using Hashira.Cards;
using Hashira.Core;
using Hashira.Entities;
using System;
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
        private UseableCardDrawer _useableCardDrawer;
        private Image _cardImage;

        [SerializeField] private TextMeshProUGUI _descriptionText, _costText;
        [SerializeField] private Image _icon;
        [SerializeField] private InputReaderSO _inputReader;

        private bool _isDrag;
        private bool _isUseable;
        private Vector2 _mousePivot;

        private void Awake()
        {
            _cardImage = GetComponent<Image>();
        }

        private void Start()
        {
            UIManager.Instance.AddUI(this);
        }

        private void Update()
        {
            MouseTracking();

            bool isUseableDistance = (_useableCardDrawer.CardUsePos.position - transform.position).sqrMagnitude < 10000;
            if (_isUseable == false && isUseableDistance)
            {
                _isUseable = true;
                _cardImage.color = Color.yellow;
            }
            else if (_isUseable == true && isUseableDistance == false)
            {
                _isUseable = false;
                _cardImage.color = Color.white;
            }
        }

        private void MouseTracking()
        {
            if (_isDrag == false) return;

            RectTransform.position = _inputReader.MousePosition + _mousePivot;
        }

        public void SetCard(CardSO cardSO, UseableCardDrawer useableCardDrawer)
        {
            _cardSO = cardSO;
            _useableCardDrawer = useableCardDrawer;
            _descriptionText.text = cardSO.cardDescription;
            _costText.text = $"{cardSO.needCost}";
            _icon.sprite = cardSO.iconSprite;
        }

        public void OnClick()
        {
            if (_isDrag) return;

            _useableCardDrawer.ExitSpread(this);
            _isDrag = true;
            _mousePivot = (Vector2)RectTransform.position - _inputReader.MousePosition;
        }

        public void OnClickEnd()
        {
            if (_isDrag == false) return;

            _isDrag = false;

            if (_isUseable)
            {
                if (Cost.TryRemoveCost(_cardSO.needCost))
                {
                    PlayerManager.Instance.Player.GetEntityComponent<EntityEffector>().AddEffect(_cardSO.GetEffectClass());
                    this.Push();
                }
                else
                {
                    Debug.Log("Cost가 부족해요");
                    _useableCardDrawer.EnterSpread(this);
                }
            }
            else
            {
                _useableCardDrawer.EnterSpread(this);
            }
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
