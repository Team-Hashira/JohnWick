using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class RubberBullet : Effect, ICoolTimeEffect
    {
        public float Duration { get; set; } = 5;
        public float Time { get; set; }
        public float Amount { get; set; }

        public override int MaxActiveCount => 1;

        public override void Enable()
        {
            base.Enable();
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
