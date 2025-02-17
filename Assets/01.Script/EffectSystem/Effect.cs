using System;
using Hashira.Entities;
using Hashira.Players;
using UnityEngine;

namespace Hashira.EffectSystem
{
    public abstract class Effect
    {
        public string name;
        public float duration;
        public float currentTime = 0;
        public int level;

        public EntityEffector baseEffector;
        public EffectUIDataSO effectUIDataSO;
        public Entity entity;
		public EntityStat entityStat;

        public event Action<float, float> CoolTimeEvent;

        public virtual void Enable()
        {

		}

        public virtual void Update()
        {
            CoolTimeEvent?.Invoke(currentTime, duration);
        }

        public virtual void Disable()
        {
            
        }
    }
}
