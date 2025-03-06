using Hashira.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    [CreateAssetMenu(fileName = "CardSetSO", menuName = "SO/CardSetSO")]
    public class CardSetSO : ScriptableObject
    {
        public List<CardSO> cardList;

        public CardSO GetRandomCard()
            => cardList[Random.Range(0, cardList.Count)];
    }
}
