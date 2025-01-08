using System;
using UnityEngine;

namespace Hashira.EffectSystem
{
    public abstract class Effect
    {
        public string name;
        public float duration;
        public float currentTime = 0;
        public int level;
        public event Action<float, float> CoolTimeEvent;  
        
        public Effect()
        {
            
        }
    }
}
