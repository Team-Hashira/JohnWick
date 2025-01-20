using UnityEngine;

namespace Hashira
{
    public class TimeManager : MonoBehaviour
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
        }

        public static void Play()
        {
            Time = _lastTime;
        }
    }
}