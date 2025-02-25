using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Hashira
{
    public static class CooldownUtillity
    {
        private static Dictionary<string, float> _timeList = new();

        public static void StartCooldown(string key)
        {
            _timeList[key] = Time.time;
        }

        public static bool CheckCooldown(string key, float duration)
        {
            return _timeList.TryGetValue(key, out float lastTime) && lastTime + duration < Time.time;
        }
    }
}
