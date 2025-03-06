using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards
{
    public class CardManager
    {
        private List<CardSO> _cardList;

        public CardManager()
        {
            _cardList = new List<CardSO>();
        }

        public void AddCard(CardSO cardSO)
        {
            _cardList.Add(cardSO);
        }
        public void RemoveCard(CardSO cardSO)
        {
            _cardList.Remove(cardSO);
        }
        //웬만하면 이거 사용
        public void RemoveCard(int index)
        {
            _cardList.RemoveAt(index);
        }

        public List<CardSO> GetCardList() => _cardList;

        public List<CardSO> GetRandomCard(int count)
        {
            List<CardSO> resultList = new List<CardSO>();

            if (_cardList.Count == 0) return resultList;

            CardSO[] cards = _cardList.ToArray();
            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, cards.Length - i);
                int randomLastIndex = cards.Length - i - 1;
                resultList.Add(cards[randomIndex]);
                CardSO temp = cards[randomIndex];
                cards[randomIndex] = cards[randomLastIndex];
                cards[randomLastIndex] = temp;

                if (randomLastIndex == 0) break;
            }
            return resultList;
        }
    }
}
