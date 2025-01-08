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
        public EffectUIDataSO effectUIDataSO;
        public event Action<float, float> CoolTimeEvent;

        public abstract void Enable();
        public abstract void Update();
        public abstract void Disable();
    }
}
