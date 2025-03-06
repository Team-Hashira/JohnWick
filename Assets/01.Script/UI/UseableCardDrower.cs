using Crogen.CrogenPooling;
using Hashira.Cards;
using Hashira.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.LatestUI
{
    public class UseableCardDrower : MonoBehaviour
    {
        [SerializeField] private int _cardCount = 4;
        public void CardDraw()
        {
            List<CardSO> cardSOList = PlayerManager.Instance.CardManager.GetRandomCard(_cardCount);
            for (int i = 0; i < cardSOList.Count; i++)
            {
                UseableCardUI useableCardUI = gameObject.Pop(UIPoolType.UseableCardUI, transform) as UseableCardUI;
                useableCardUI.SetCard(cardSOList[i]);
            }
        }
    }
}
