using UnityEngine;

namespace Hashira.Core.EventSystem
{
    public static class SoundEvents
    {
        public static SoundGeneratedEvent SoundGeneratedEvent = new SoundGeneratedEvent();
    }

    public class SoundGeneratedEvent : GameEvent
    {
        public Transform origin;
        public float loudness;
        public bool isContinuous;
    }
}
