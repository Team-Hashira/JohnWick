using UnityEngine;

namespace Hashira
{
    public static class TimeManager
    {
        private static float _lastTime = 0;
        
        public static float Time 
        {
            get => UnityEngine.Time.timeScale;
            set
            {
                if (Mathf.Approximately(0, value))
                {
                    Debug.LogWarning("Please Use Method");
                }
            }
        }
        
        public static void Pause()
        {
            _lastTime = Time;
            Time = 0;
        }

        public static void Play()
        {
            Time = _lastTime;
        }
    }
}