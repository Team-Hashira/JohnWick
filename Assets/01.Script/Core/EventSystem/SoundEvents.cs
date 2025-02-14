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
        public ESoundSource soundSource;
        public Vector3 originPosition;
        public float loudness;
    }

    public class NearbySoundPointEvent : GameEvent
    {
        public Node node;
        public ESoundSource soundSource;
        public Vector3 originPosition;
        public float loudness;
    }

    public enum ESoundSource
    {
        Player,
        Enemy,
        Gun,
        FootStep,
        Melee,
        Nature,
    }
}
