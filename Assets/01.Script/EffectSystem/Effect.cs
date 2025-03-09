using Hashira.Cards;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.EffectSystem
{
    public abstract class Effect
    {
        public string name;
        public virtual int MaxActiveCount { get; private set; } = 1;
        public bool Visable { get; private set; } = true;
        public CardSO CardSO { get; private set; }

        public EntityEffector entityEffector;
		public EntityStat entityStat;
        public Attacker attacker;

        public void SetCardSO(CardSO cardSO)
        {
            CardSO = cardSO;
        }

        public virtual void Enable()
        {
            attacker = GameObject.FindFirstObjectByType<Attacker>();
        }

        public virtual void Update()
        {
        }

        public virtual void Disable()
        {
        }
    }
}
