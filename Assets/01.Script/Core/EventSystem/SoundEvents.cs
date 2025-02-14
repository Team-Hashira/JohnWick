using Hashira.Pathfind;
using System;
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

    [Flags]
    public enum ESoundSource
    {
        Player = 1,
        Enemy = 1 << 1,
        Gun = 1 << 2,
        FootStep = 1 << 3,
        Melee = 1 << 4,
        Nature = 1 << 5,
    }
}
