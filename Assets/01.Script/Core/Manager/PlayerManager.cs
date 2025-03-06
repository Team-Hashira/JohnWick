using Hashira.Cards;
using Hashira.Players;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Core
{
    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        private Player _player;
        public Player Player 
        {
            get
            {
                if (_player == null)
                {
                    _player = FindFirstObjectByType<Player>();
                }
                return _player;
            }
        }
        private CardManager _cardManager;
        public CardManager CardManager
        {
            get
            {
                if (_cardManager == null)
                {
                    _cardManager = new CardManager();
                }
                return _cardManager;
            }
        }
    }
}
