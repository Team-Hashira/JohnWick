using DG.Tweening;
using Hashira.Cards;
using Hashira.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.LatestUI
{
    public class SelectableCardUI : UIBase, IClickableUI, IHoverableUI
    {
        [field: SerializeField]
        public Collider2D Collider { get; set; }

        private CardSO _cardSO;

        private LayoutElement _layoutElement;

        private SelectingCardPanel _selectingCardPanel;

        private Vector2 _defaultPosition;
        private Vector2 _defaultSize;

        private Sequence _setDefaultTween;
        private Sequence _hoverSequence;
        private Sequence _selectSequence;

        public void Reload()
        {
            _layoutElement.ignoreLayout = false;
            RectTransform.sizeDelta = _defaultSize;
            _cardSO = PlayerManager.Instance.CardManager.GetRandomCard(1)[0];
        }

        public void Initialize(SelectingCardPanel panel)
        {
            _layoutElement = GetComponent<LayoutElement>();
            _selectingCardPanel = panel;
            _defaultPosition = RectTransform.anchoredPosition;
            _defaultSize = RectTransform.sizeDelta;
        }

        public void OnClick()
        {
            _selectSequence = DOTween.Sequence();
            _layoutElement.ignoreLayout = true;
            _selectSequence.Append(RectTransform.DOSizeDelta(_defaultSize * 2.0f, 0.3f).SetEase(Ease.OutQuint))
                .Join(RectTransform.DORotate(new Vector3(0, 360f), 0.3f, RotateMode.FastBeyond360))
                .JoinCallback(() => _selectingCardPanel.Select(this));
        }

        public void OnClickEnd()
        {
        }

        public void OnCursorEnter()
        {
            if (_setDefaultTween != null && _setDefaultTween.IsActive())
                _setDefaultTween.Kill();
            _hoverSequence = DOTween.Sequence();
            _hoverSequence.Append(RectTransform.DOSizeDelta(_defaultSize * new Vector2(1.5f, 1.5f), 0.3f));
        }

        public void OnCursorExit()
        {
            if (_hoverSequence != null && _hoverSequence.IsActive())
                _hoverSequence.Kill();
            _setDefaultTween = DOTween.Sequence();
            _setDefaultTween.Append(RectTransform.DOSizeDelta(_defaultSize, 0.3f));
        }

        public void Wipe(int direction)
        {
            float x = direction == 1 ? Screen.width * 1.5f : -Screen.width * 0.5f;
            RectTransform.DOAnchorPosX(x, 0.6f).SetEase(Ease.InBack);

            _layoutElement.ignoreLayout = true;
        }
    }
}
