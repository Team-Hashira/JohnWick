using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    public static class TimeManager
    {
        private static Stack<float> _timeScaleChangedStack = new();
        
        public static float TimeScale
        {
            get => Time.timeScale;
            set
            {
                if (Mathf.Approximately(0, value))
                {
                    Debug.LogWarning("Please Use Method");
                }
            }
        }
        
        public static void SetTimeScale(float value)
        {
            _timeScaleChangedStack.Push(TimeScale);
			Time.timeScale = value;
            Time.fixedDeltaTime = 0.002f * Time.timeScale;
        }

        public static void UndoTimeScale()
        {
            if (_timeScaleChangedStack.Count == 0) return;
			Time.timeScale = _timeScaleChangedStack.Pop();
            Time.fixedDeltaTime = 0.002f * Time.timeScale;
        }

        public static void ResetTimeScale()
        {
            while (_timeScaleChangedStack.Count > 0)
            {
                Time.timeScale = _timeScaleChangedStack.Pop();
                Time.fixedDeltaTime = 0.002f * Time.timeScale;
            }
        }
    }
}