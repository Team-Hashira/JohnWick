using Crogen.CrogenPooling;
using Hashira.Cards;
using Hashira.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.LatestUI
{
    public class UseableCardDrawer : MonoBehaviour
    {
        [SerializeField] private int _reroll = 10;
        [SerializeField] private int _cardCount = 4;

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
                useableCardUI.SetCard(cardSOList[i]);
                _useableCardUILsit.Add(useableCardUI);
            }
        }
        public void Reroll()
        {
            if (Cost.TryRemoveCost(_reroll))
            {
                CardDraw();
            }
        }
    }
}
