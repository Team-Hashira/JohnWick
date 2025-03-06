using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class MultipleShot : Effect, ICoolTimeEffect
    {
        public float Duration { get; set; } = 5;
        public float Time { get; set; }

        public override int MaxActiveCount => 4;

        public override void Enable()
        {
            base.Enable();
        }

        public override void Update()
        {
            base.Update();
            if(Input.GetKeyDown(KeyCode.H))
            {
            }
        }

        public override void Disable()
        {
            base.Disable();
        }

        public void OnTimeOut()
        {
        }
    }
}
