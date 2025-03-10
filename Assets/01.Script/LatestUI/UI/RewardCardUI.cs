using DG.Tweening;
using Hashira.Cards;
using Hashira.Core;
using Hashira.Core.Attribute;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Hashira.LatestUI
{
    public class RewardCardUI : UIBase, IClickableUI, IHoverableUI
    {
        [field: SerializeField]
        public Collider2D Collider { get; set; }
        public string Key { get; set; }

        private CardSO _cardSO;

        private RewardCardPanel _selectingCardPanel;

        [SerializeField]
        private Vector2 _defaultPosition;
        private Vector2 _defaultScale;

        private Sequence _setDefaultTween;
        private Sequence _hoverSequence;
        private Sequence _selectSequence;

        [SerializeField]
        private Image _cardImage;
        [SerializeField]
        private TextMeshProUGUI _descrptionText, _costText;

        //[Header("Test")]
        //[Dependent]
        //[SerializeField]
        //private CardSO _input;


        public void Reload(CardSO cardSO)
        {
            Collider.enabled = false;
            RectTransform.localScale = _defaultScale;
            _cardSO = cardSO;
            _descrptionText.text = _cardSO.cardDescription;
            _costText.text = $"{_cardSO.needCost}";
            _cardImage.sprite = _cardSO.cardSprite;
            Vector2 randomPos = Random.insideUnitCircle.normalized;
            RectTransform.anchoredPosition = Vector2.zero;
            StartCoroutine(ReloadCoroutine(_defaultPosition, 0.7f, () =>
            {
                Collider.enabled = true;
            }));
        }

        public void Initialize(RewardCardPanel panel)
        {
            Collider.enabled = false;
            _selectingCardPanel = panel;
            _defaultPosition = RectTransform.anchoredPosition;
            _defaultScale = RectTransform.localScale;
        }

        public void OnClick()
        {
            _selectSequence = DOTween.Sequence();
            Collider.enabled = false;
            _selectSequence
                .Append(RectTransform.DORotate(new Vector3(0, 360f), 0.3f, RotateMode.FastBeyond360))
                .JoinCallback(() => _selectingCardPanel.Select(this))
                .InsertCallback(_selectSequence.Duration() - 0.1f, _selectingCardPanel.Close)
                //카드 선택으로
                .InsertCallback(0.5f, GameManager.Instance.StartCardUse);

            PlayerManager.Instance.CardManager.AddCard(_cardSO);
        }

        public void OnClickEnd()
        {
        }

        public void OnCursorEnter()
        {
            if (_setDefaultTween != null && _setDefaultTween.IsActive())
                _setDefaultTween.Kill();
            _hoverSequence = DOTween.Sequence();
            _hoverSequence.Append(RectTransform.DOScale(_defaultScale * new Vector2(1.5f, 1.5f), 0.3f));
        }

        public void OnCursorExit()
        {
            if (_hoverSequence != null && _hoverSequence.IsActive())
                _hoverSequence.Kill();
            _setDefaultTween = DOTween.Sequence();
            _setDefaultTween.Append(RectTransform.DOScale(_defaultScale, 0.3f));
        }

        public void Wipe(int direction)
        {
            Collider.enabled = false;
            float x = direction == 1 ? Screen.width * 1.5f : -Screen.width * 0.5f;
            RectTransform.DOAnchorPosX(x, 0.6f).SetEase(Ease.InBack);
        }

        private IEnumerator ReloadCoroutine(Vector2 destination, float duration, Action OnComplete = null)
        {
            Vector2 startPos = RectTransform.anchoredPosition;
            //Vector2 randomPos = Random.insideUnitCircle.normalized;
            //randomPos = destination + randomPos * (Screen.width * 0.5f);
            float toAdd = 1f / duration;
            float percent = 0;
            while (percent < 1f)
            {
                float t = MathEx.OutCubic(percent);
                //RectTransform.anchoredPosition = MathEx.Bezier(t, startPos, randomPos, destination);
                RectTransform.anchoredPosition = MathEx.Bezier(t, startPos, destination);
                yield return null;
                percent += Time.deltaTime * toAdd;
            }
            OnComplete?.Invoke();
        }
    }
}
