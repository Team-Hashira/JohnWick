using System;
using UnityEngine;

namespace Hashira.EffectSystem
{
    public interface ICoolTimeEffect    
    {
        public float Duration { get; set; }
        public float Time { get; set; }

        public void OnTimeOut();
    }
}
