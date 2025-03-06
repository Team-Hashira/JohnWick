using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

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

        private void Start()
        {
            SetActive(false, 0);
        }

        private void Update()
        {
            if (Keyboard.current.uKey.wasPressedThisFrame)
            {
                Open();
            }
        }

        public void Open()
        {
            SetActive(true, 0);
            foreach (var card in _selectableCards)
            {
                //card.Reload();
            }
        }

        public void Close()
        {
            SetActive(false);
        }

        public void SetActive(bool isActive, float duration = 0.5f)
        {
            float alpha = isActive ? 1f : 0;
            _canvasGroup.DOFade(alpha, duration);
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = true;
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
