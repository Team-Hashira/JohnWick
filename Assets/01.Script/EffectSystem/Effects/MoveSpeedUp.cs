using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class MoveSpeedUp : Effect
    {
        public override void Enable()
        {
            Debug.Log("Starting SpeedUp");   
        }

        public override void Update()
        {
            Debug.Log("Moving SpeedUp");
        }

        public override void Disable()
        {
            Debug.Log("Stopping SpeedUp");
        }
    }
}
