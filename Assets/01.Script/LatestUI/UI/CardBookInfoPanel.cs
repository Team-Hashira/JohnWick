using DG.Tweening;
using Hashira.Cards;
using System;
using UnityEngine;
using static UnityEngine.UI.Toggle;

namespace Hashira.LatestUI
{
    public class CardBookInfoPanel : UIBase, IToggleUI
    {
        [field: SerializeField]
        public string Key { get; set; }

        public event OnToggleEvent OnToggleEvent;

        private float _xSize = Screen.width * 0.37f;

        private void Awake()
        {
            RectTransform.sizeDelta = new Vector2(_xSize, RectTransform.sizeDelta.y);
        }

        public void SetInfo(CardSO card)
        {

        }

        public void Open()
        {
            OnToggleEvent?.Invoke(true);
            RectTransform.DOAnchorPosX(0, 0.4f);
        }

        public void Close()
        {
            OnToggleEvent?.Invoke(false);
            RectTransform.DOAnchorPosX(_xSize, 0.4f);
        }
    }
}
