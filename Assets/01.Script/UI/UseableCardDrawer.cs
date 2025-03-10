using Crogen.CrogenPooling;
using Hashira.Cards;
using Hashira.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.LatestUI
{
    public class UseableCardDrawer : UIBase
    {
        [SerializeField] private int _cardCount = 4;

        [SerializeField] private int _interval = 10;
        [SerializeField] private int _spreadMovingSpeed = 10;

        [field: SerializeField] public Transform CardUsePos { get; private set; }

        private List<UseableCardUI> _useableCardUILsit = new List<UseableCardUI>();

        public void CardDraw()
        {
            _useableCardUILsit.ForEach(cardUI =>
            {
                cardUI.Push();
            });
            _useableCardUILsit.Clear();
            List<CardSO> cardSOList = PlayerManager.Instance.CardManager.GetRandomCard(_cardCount);
            for (int i = 0; i < cardSOList.Count; i++)
            {
                UseableCardUI useableCardUI = gameObject.Pop(UIPoolType.UseableCardUI, transform) as UseableCardUI;
                useableCardUI.SetCard(cardSOList[i], this);
                EnterSpread(useableCardUI);
            }
        }

        public void ExitSpread(UseableCardUI useableCardUI)
        {
            _useableCardUILsit.Remove(useableCardUI);
        }

        public void EnterSpread(UseableCardUI useableCardUI)
        {
            _useableCardUILsit.Add(useableCardUI);
        }

        private void Update()
        {
            int cardCount = _useableCardUILsit.Count;
            float interval = (float)_interval / 1920 * Screen.width * ((1920f / 1080) / ((float)Screen.width / Screen.height));
            for (int i = 0; i < cardCount; i++)
            {
                Vector2 currentPos = _useableCardUILsit[i].transform.position;
                Vector2 targetPos = transform.position + Vector3.right * (i + 0.5f - (float)cardCount / 2) * interval;
                _useableCardUILsit[i].transform.position = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * _spreadMovingSpeed);
            }
        }
    }
}
