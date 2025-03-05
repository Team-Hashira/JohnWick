using Hashira.Cards;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    public class CardManager : MonoSingleton<CardManager>
    {
        public List<CardSO> cardList;

        public void AddCard(CardSO cardSO)
        {
            cardList.Add(cardSO);
        }
        public void RemoveCard(CardSO cardSO)
        {
            cardList.Remove(cardSO);
        }
        //웬만하면 이거 사용
        public void RemoveCard(int index)
        {
            cardList.RemoveAt(index);
        }

        public List<CardSO> GetCardList() => cardList;

        public List<CardSO> GetRandomCard(int count)
        {
            List<CardSO> resultList = new List<CardSO>();
            CardSO[] cards = cardList.ToArray();
            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, cards.Length - i);
                resultList.Add(cards[randomIndex]);
                CardSO temp = cards[randomIndex];
                cards[randomIndex] = cards[cards.Length - i - 1];
                cards[cards.Length - i - 1] = temp;
            }
            return resultList;
        }
    }
}
