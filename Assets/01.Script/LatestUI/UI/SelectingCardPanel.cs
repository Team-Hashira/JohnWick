using DG.Tweening;
using UnityEngine;

namespace Hashira.LatestUI
{
    public class SelectingCardPanel : UIBase, IToggleUI
    {
        [field: SerializeField]
        public string Key { get; set; }

        [SerializeField]
        private SelectableCardUI[] _selectableCards;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            foreach (var card in _selectableCards)
            {
                card.Initialize(this);
            }
        }

        public void Open()
        {
            foreach (var card in _selectableCards)
            {
                card.Reload();
            }
        }

        public void Close()
        {
            _canvasGroup.DOFade(0, 0.5f);
        }

        public void Select(SelectableCardUI card)
        {
            foreach(var c in _selectableCards)
            {
                if (c == card)
                    continue;
                if(c.RectTransform.anchoredPosition.x > card.RectTransform.anchoredPosition.x)
                    c.Wipe(1);
                else
                    c.Wipe(-1);
            }
        }
    }
}
