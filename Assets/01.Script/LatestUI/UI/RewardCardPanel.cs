using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hashira.LatestUI
{
    public class RewardCardPanel : UIBase, IToggleUI
    {
        [field: SerializeField]
        public string Key { get; set; }

        [SerializeField]
        private float _space = 250f;
        [SerializeField]
        private RewardCardUI[] _rewardCards;
        [SerializeField]
        private CardSetSO _cardSet;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            for(int i = -1; i <= 1; i++)
            {
                float space = _space * i;
                _rewardCards[i + 1].RectTransform.anchoredPosition = new Vector3(space, 0);
            }

            foreach (var card in _rewardCards)
            {
                card.Initialize(this);
            }

        }

        private void Start()
        {
            SetActive(false, 0);
        }

        public void Open()
        {
            SetActive(true, 0);
            foreach (var card in _rewardCards)
            {
                card.Reload(_cardSet.GetRandomCard());
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

        public void Select(RewardCardUI card)
        {
            foreach(var c in _rewardCards)
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
