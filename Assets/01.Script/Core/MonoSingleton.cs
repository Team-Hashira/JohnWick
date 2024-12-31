using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindFirstObjectByType<T>();
                if (_Instance == null)
                {
                    Debug.LogError($"[ {typeof(T).ToString()} ] object is not found.");
                }
            }
            return _Instance;
        }
    }
}
