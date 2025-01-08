using System;
using Hashira.Entities;
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
        public event Action<float, float> CoolTimeEvent;

        public abstract void Enable();

        public virtual void Update()
        {
            CoolTimeEvent?.Invoke(currentTime, duration);
        }
        public abstract void Disable();
    }
}
