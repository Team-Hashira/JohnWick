using System;
using Hashira.Entities;

namespace Hashira.EffectSystem
{
    public abstract class Effect
    {
        public string name;
        public float duration;
        public float currentTime = 0;
        public int level;
        public bool isCounting = false;
        public bool isLoop = false;

        public int maxCount = 10;
        protected int _count = 0;
        public int Count 
        { 
            get => _count;
            set 
            { 
                _count = value;
                OnCounting(_count);
            }
        }

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

        public virtual void OnCounting(int count)
        {

        }
    }
}
