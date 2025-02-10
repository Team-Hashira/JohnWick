using Hashira.Pathfind;
using UnityEngine;

namespace Hashira.Core.EventSystem
{
    public static class SoundEvents
    {
        public static SoundGeneratedEvent SoundGeneratedEvent = new SoundGeneratedEvent();
        public static NearbySoundPointEvent NearbySoundPointEvent = new NearbySoundPointEvent();
    }

    public class SoundGeneratedEvent : GameEvent
    {
        public Transform origin;
        public Vector3 originPosition;
        public float loudness;
        public bool isContinuous;
    }

    public class NearbySoundPointEvent : GameEvent
    {
        public Node node;
        public Vector3 originPosition;
        public float loudness;
    }
}
