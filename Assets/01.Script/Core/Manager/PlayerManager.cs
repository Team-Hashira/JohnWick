using Hashira.Cards;
using Hashira.Players;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Core
{
    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        public Player Player { get; private set; }

        public CardManager CardManager { get; private set; }

        private void Awake()
        {
            Player = FindFirstObjectByType<Player>();
            CardManager = new CardManager();
        }
    }
}
